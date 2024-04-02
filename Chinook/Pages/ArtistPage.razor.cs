using Chinook.ClientModels;
using Chinook.Helpers;
using Chinook.Shared.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Chinook.Models;
using Chinook.Common;

namespace Chinook.Pages
{
    public partial class ArtistPage
    {
        [Parameter] public long ArtistId { get; set; }
        [CascadingParameter] private Task<AuthenticationState>? authenticationState { get; set; }
        private Modal? PlaylistDialog { get; set; }

        private ClientModels.Artist? Artist;
        private List<PlaylistTrack>? Tracks;
        private PlaylistTrack? SelectedTrack;
        private string? CurrentUserId;
        private List<ClientModels.Playlist>? Playlists;
        private string? NewPlaylistName;
        private long SelectedPlaylist;
        bool IsFormInvalid => SelectedPlaylist == 0 && string.IsNullOrEmpty(NewPlaylistName);

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await InvokeAsync(StateHasChanged);
                CurrentUserId = await GetUserId() ?? throw new Exception(Constants.UserNotFoundMessage);
                Artist = await ArtistService.GetArtistByIdAsync(ArtistId);
                Tracks = await PlaylistService.GetTracksByArtistIdAsync(ArtistId, CurrentUserId);
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        private async Task LoadPlaylists()
        {
            try
            {
                Playlists = CurrentUserId == null ? new List<ClientModels.Playlist>() : await PlaylistService.GetPlaylistsByUserIdAsync(CurrentUserId);
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }

        }

        private async Task<string> GetUserId()
        {
            try
            {
                var authenticationStateValue = await authenticationState;

                if (authenticationStateValue != null)
                {
                    var user = authenticationStateValue.User;

                    if (user != null)
                    {
                        var userIdClaim = user.FindFirst(u => u.Type.Contains(ClaimTypes.NameIdentifier));

                        if (userIdClaim != null)
                        {
                            CloseErrorMessage();
                            return userIdClaim.Value;
                        }
                        else
                        {
                            throw new Exception(Constants.UserIdClaimNotFoundMessage);
                        }
                    }
                    else
                    {
                        throw new Exception(Constants.UserNotFoundMessage);
                    }
                }
                else
                {
                    throw new Exception(Constants.AuthenticationStateNotFoundMessage);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
                return string.Empty;
            }
        }

        private async Task FavoriteTrack(long trackId)
        {
            try
            {
                var track = Tracks?.FirstOrDefault(t => t.TrackId == trackId) ?? throw new Exception(Constants.TrackNotFoundMessage);
                await PlaylistService.FavoriteTrackAsync(trackId, CurrentUserId);
                Tracks = await PlaylistService.GetTracksByArtistIdAsync(ArtistId, CurrentUserId);
                InfoMessage = MessageGenerator.GenerateAssignTrackInfoMessage(track, Constants.Favorites);
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        private async Task UnfavoriteTrack(long trackId)
        {
            try
            {
                var track = Tracks?.FirstOrDefault(t => t.TrackId == trackId) ?? throw new Exception(Constants.TrackNotFoundMessage);
                await PlaylistService.UnfavoriteTrackAsync(trackId, CurrentUserId);
                Tracks = await PlaylistService.GetTracksByArtistIdAsync(ArtistId, CurrentUserId);
                InfoMessage = MessageGenerator.GenerateRemoveTrackInfoMessage(track, Constants.Favorites);
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }

        }

        private async Task OpenPlaylistDialog(long trackId)
        {
            await LoadPlaylists();
            CloseInfoMessage();
            SelectedTrack = Tracks?.FirstOrDefault(t => t.TrackId == trackId);
            PlaylistDialog?.Open();
        }

        private async Task AddTrackToPlaylist()
        {
            if (IsFormInvalid)
            {
                HandleError(Constants.PlaylistInputAndSelectMessage);
                return;
            }

            CloseInfoMessage();
            string errors = await ProcessPlaylistActions();

            if (!string.IsNullOrEmpty(errors))
            {
                HandleError(errors);
            }
            else
            {
                ClearInputs();
                PlaylistDialog?.Close();
                CloseErrorMessage();
            }
        }

        private async Task<string> ProcessPlaylistActions()
        {
            string errors = string.Empty;

            if (!string.IsNullOrWhiteSpace(NewPlaylistName))
            {
                errors = await AddTrackToNewPlaylist(NewPlaylistName);
            }

            if (SelectedPlaylist != 0 && Playlists != null)
            {
                errors += (errors != string.Empty ? Constants.LineBrake : string.Empty) + await AddTrackToExistingPlaylist();
            }

            return errors;
        }

        private async Task<string> AddTrackToNewPlaylist(string newPlaylistName)
        {
            try
            {
                long playlistId = await PlaylistService.AddTrackToNewPlaylist(SelectedTrack.TrackId, newPlaylistName, CurrentUserId);
                AddInfoMessage(MessageGenerator.GenerateNewPlaylistInfoMessage(SelectedTrack, newPlaylistName));
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format(Constants.PlaylistCreationErrorMessage, newPlaylistName, ex.Message);
            }
        }

        private async Task<string> AddTrackToExistingPlaylist()
        {
            try
            {
                var playlist = GetPlaylist();
                if (playlist != null)
                {
                    await PlaylistService.AddTrackToExistingPlaylist(SelectedPlaylist, SelectedTrack.TrackId);
                    AddInfoMessage(MessageGenerator.GenerateNewPlaylistInfoMessage(SelectedTrack, playlist.Name));
                    return string.Empty;
                }
                else
                {
                    return Constants.PlaylistNotFoundMessage;
                }
            }
            catch (Exception ex)
            {
                var playlist = GetPlaylist();
                return string.Format(Constants.PlaylistTracksAdditionErrorMessage, playlist?.Name, ex.Message);
            }
        }

        private void AddInfoMessage(string message)
        {
            if (!string.IsNullOrEmpty(InfoMessage))
            {
                InfoMessage += Constants.LineBrake;
            }
            InfoMessage += message;
        }

        private void ClearInputs()
        {
            NewPlaylistName = string.Empty;
            SelectedPlaylist = 0;
        }

        private ClientModels.Playlist? GetPlaylist()
        {
            return Playlists?.Find(x => x.PlaylistId == SelectedPlaylist);
        }
    }
}

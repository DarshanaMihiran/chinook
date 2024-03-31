using Chinook.ClientModels;
using Chinook.Helpers;
using Chinook.Shared.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace Chinook.Pages
{
    public partial class ArtistPage
    {
        [Parameter] public long ArtistId { get; set; }
        [CascadingParameter] private Task<AuthenticationState>? authenticationState { get; set; }
        private Modal? PlaylistDialog { get; set; }

        private ClientModels.ArtistClientModel? Artist;
        private List<PlaylistTrack>? Tracks;
        private PlaylistTrack? SelectedTrack;
        private string? CurrentUserId;
        private List<ClientModels.Playlist>? Playlists;
        private string? NewPlaylistName;
        private long SelectedPlaylist;
        bool IsNewPlaylistDisabled { get; set; } = false;
        bool IsFormInvalid => SelectedPlaylist == 0 && string.IsNullOrEmpty(NewPlaylistName);

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await InvokeAsync(StateHasChanged);
                CurrentUserId = await GetUserId() ?? throw new Exception("user not found");
                Artist = await ArtistService.GetArtistByIdAsync(ArtistId);
                Tracks = await PlaylistService.GetTracksByArtistIdAsync(ArtistId, CurrentUserId);
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        void HandleDropdownInput(ChangeEventArgs e)
        {
            if (e.Value?.ToString() == "0")
            {
                IsNewPlaylistDisabled = false;
            }
            else
            {
                IsNewPlaylistDisabled = true;
                NewPlaylistName = string.Empty;
            }
        }

        void HandleNewPlaylistInput(ChangeEventArgs e)
        {
            var inputText = e.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(inputText))
            {
                SelectedPlaylist = 0;
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
                            throw new Exception("userIdClaim not found");
                        }
                    }
                    else
                    {
                        throw new Exception("user not found");
                    }
                }
                else
                {
                    throw new Exception("authenticationState not found");
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
                var track = Tracks?.FirstOrDefault(t => t.TrackId == trackId) ?? throw new Exception("track not found");
                await PlaylistService.FavoriteTrackAsync(trackId, CurrentUserId);
                Tracks = await PlaylistService.GetTracksByArtistIdAsync(ArtistId, CurrentUserId);
                InfoMessage = MessageGenerator.GenerateAssignTrackInfoMessage(track, "Favorites");
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
                var track = Tracks?.FirstOrDefault(t => t.TrackId == trackId) ?? throw new Exception("track not found");
                await PlaylistService.UnfavoriteTrackAsync(trackId, CurrentUserId);
                Tracks = await PlaylistService.GetTracksByArtistIdAsync(ArtistId, CurrentUserId);
                InfoMessage = MessageGenerator.GenerateRemoveTrackInfoMessage(track, "Favorites");
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
                HandleError("Please select an existing playlist or enter a name for the new playlist.");
                return;
            }
              
            CloseInfoMessage();
            long playlistId = 0;
            try
            {
                if (SelectedPlaylist == 0)
                {
                    if (!string.IsNullOrWhiteSpace(NewPlaylistName))
                    {
                        playlistId = await PlaylistService.AddTrackToNewPlaylist(SelectedTrack.TrackId, NewPlaylistName, CurrentUserId);
                    }
                    InfoMessage = MessageGenerator.GenerateNewPlaylistInfoMessage(SelectedTrack, NewPlaylistName);
                }
                else
                {
                    if (Playlists != null)
                    {
                        var playlist = Playlists.Find(x => x.PlaylistId == SelectedPlaylist);
                        if (playlist != null)
                        {
                            await PlaylistService.AddTrackToExistingPlaylist(SelectedPlaylist, SelectedTrack.TrackId);
                            InfoMessage = MessageGenerator.GenerateNewPlaylistInfoMessage(SelectedTrack, playlist.Name);
                        }
                        else
                        {
                            throw new Exception("Playlist not found");
                        }
                    }
                    else
                    {
                        throw new Exception("Playlist not found");
                    }
                }
                ClearInputs();
                PlaylistDialog?.Close();
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        private void ClearInputs()
        {
            NewPlaylistName = string.Empty;
            SelectedPlaylist = 0;
        }
    }
}

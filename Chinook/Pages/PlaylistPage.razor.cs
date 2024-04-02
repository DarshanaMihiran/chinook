using Chinook.ClientModels;
using Chinook.Common;
using Chinook.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Chinook.Pages
{
    public partial class PlaylistPage
    {
        [Parameter] public long PlaylistId { get; set; }
        [CascadingParameter] private Task<AuthenticationState>? authenticationState { get; set; }

        private Chinook.ClientModels.Playlist? Playlist;
        private string? CurrentUserId;
        private string? playlistName;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                CurrentUserId = await GetUserId() ?? throw new Exception(Constants.UserNotFoundMessage);
                await InvokeAsync(StateHasChanged);

                Playlist = await PlaylistService.GetPlaylistById(PlaylistId, CurrentUserId);
                SetPlaylistName();

                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        private async Task<string> GetUserId()
        {
            var user = await authenticationState;
            var userIdClaim = user.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value;
            return userId ?? string.Empty;
        }

        private async Task FavoriteTrack(long trackId)
        {
            try
            {
                await UpdatePlaylistAfterAction(trackId, async () =>
                {
                    await PlaylistService.FavoriteTrackAsync(trackId, CurrentUserId);
                    InfoMessage = MessageGenerator.GenerateAssignTrackInfoMessage(GetTrack(trackId), Constants.Favorites);
                });
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
                await UpdatePlaylistAfterAction(trackId, async () =>
                {
                    await PlaylistService.UnfavoriteTrackAsync(trackId, CurrentUserId);
                    InfoMessage = MessageGenerator.GenerateRemoveTrackInfoMessage(GetTrack(trackId), Constants.Favorites);
                });
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        private async Task UpdatePlaylistAfterAction(long trackId, Func<Task> action)
        {
            await action();
            Playlist = await PlaylistService.GetPlaylistById(PlaylistId, CurrentUserId);
            CloseErrorMessage();
        }

        private async Task RemoveTrack(long trackId)
        {
            try
            {
                await UpdatePlaylistAfterAction(trackId, async () =>
                {
                    var track = GetTrack(trackId);
                    await PlaylistService.RemoveTrackFromPlaylist(trackId, PlaylistId);
                    InfoMessage = MessageGenerator.GenerateRemoveTrackInfoMessage(track, Playlist.Name);
                });
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        private PlaylistTrack GetTrack(long trackId)
        {
            return Playlist?.Tracks?.FirstOrDefault(t => t.TrackId == trackId) ?? throw new Exception(Constants.TrackNotFoundMessage);
        }

        private void SetPlaylistName()
        {
            playlistName = Playlist?.Name == Constants.Favorites ? Constants.MyFavoriteTracksMessage : Playlist?.Name;
        }
    }
}

using Chinook.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Chinook.ClientModels;
using System.Security.Claims;

namespace Chinook.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter] private Task<AuthenticationState>? authenticationState { get; set; }
        [Inject]
        private IPlaylistService? PlaylistService { get; set; }
        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private List<Playlist> PlaylistItems = new List<Playlist>();
        private long? favoriteId;
        private string? CurrentUserId;
        private bool collapseNavMenu = true;
        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                CurrentUserId = await GetUserId() ?? throw new Exception("User not found");
                PlaylistItems = await PlaylistService.GetPlaylistsByUserIdAsync(CurrentUserId) ?? throw new Exception("PlaylistItems not found");
                favoriteId = PlaylistItems.Where(x => x.Name == "Favorites").FirstOrDefault()?.PlaylistId;
                PlaylistService.PlaylistAdded += HandlePlaylistAdded;
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }

        private string RenderPlaylistName(string name)
        {
            return name == "Favorites" ? "My Favorite Tracks" : name;
        }

        private async void HandlePlaylistAdded(object sender, EventArgs e)
        {
            try
            {
                PlaylistItems = await PlaylistService.GetPlaylistsByUserIdAsync(CurrentUserId);
                StateHasChanged();
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }

        }

        protected void Dispose(bool disposing)
        {
            PlaylistService.PlaylistAdded -= HandlePlaylistAdded;
        }

        private async Task<string?> GetUserId()
        {
            try
            {
                var user = (await authenticationState).User ?? throw new Exception("user not found");
                var userId = user.FindFirst(u => u.Type.Contains(ClaimTypes.NameIdentifier))?.Value;
                CloseErrorMessage();
                return userId;
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
                return string.Empty;
            }
        }

        private void NavigateToPlaylist(long playlistId)
        {
            NavigationManager?.NavigateTo($"playlist/{playlistId}", true);
        }

        private void NavigateToFavorites()
        {
            NavigationManager?.NavigateTo($"playlist/{favoriteId}", true);
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}

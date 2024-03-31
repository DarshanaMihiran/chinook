using Chinook.ClientModels;
using Microsoft.AspNetCore.Components;

namespace Chinook.Pages
{
    public partial class Home
    {
        private List<ArtistClientModel>? Artists;
        private string? searchTerm;

        protected override async Task OnInitializedAsync()
        {
            await InvokeAsync(StateHasChanged);
            await LoadArtists();
        }

        private async Task SearchArtists(ChangeEventArgs e)
        {
            searchTerm = e.Value.ToString();
            await LoadArtists();
        }

        private async Task LoadArtists()
        {
            try
            {
                Artists = string.IsNullOrWhiteSpace(searchTerm) ?
                    await ArtistsService.GetArtistsAsync() :
                    await ArtistsService.GetArtistsBySearchAsync(searchTerm);
                CloseErrorMessage();
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }
    }
}

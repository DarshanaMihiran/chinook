using Chinook.ClientModels;

namespace Chinook.Services.Interfaces
{
    public interface IArtistService
    {
        Task<List<ArtistClientModel>> GetArtistsAsync();
        Task<List<ArtistClientModel>> GetArtistsBySearchAsync(string searchTerm);
        Task<ArtistClientModel> GetArtistByIdAsync(long artistId);
        Task<List<AlbumClientModel>> GetAlbumsByArtistIdAsync(int artistId);
    }
}

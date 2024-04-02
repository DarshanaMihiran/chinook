using Chinook.ClientModels;

namespace Chinook.Services.Interfaces
{
    public interface IArtistService
    {
        Task<List<Artist>> GetArtistsAsync();
        Task<List<Artist>> GetArtistsBySearchAsync(string searchTerm);
        Task<Artist> GetArtistByIdAsync(long artistId);
        Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId);
    }
}

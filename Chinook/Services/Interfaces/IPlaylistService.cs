using Chinook.ClientModels;

namespace Chinook.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long artistId, string currentUserId);
        Task FavoriteTrackAsync(long trackId, string currentUserId);
        Task UnfavoriteTrackAsync(long trackId, string currentUserId);
        Task<List<ClientModels.Playlist>> GetPlaylistsByUserIdAsync(string currentUserId);
        Task<long> AddTrackToThePlaylist(long trackId, string PlaylistName, string currentUserId);
    }
}

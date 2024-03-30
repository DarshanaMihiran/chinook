using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Services.Interfaces
{
    public interface IPlaylistService
    {
        event EventHandler PlaylistAdded;
        Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long artistId, string currentUserId);
        Task FavoriteTrackAsync(long trackId, string currentUserId);
        Task UnfavoriteTrackAsync(long trackId, string currentUserId);
        Task<List<ClientModels.Playlist>> GetPlaylistsByUserIdAsync(string currentUserId);
        Task<long> AddTrackToNewPlaylist(long trackId, string PlaylistName, string currentUserId);
        Task AddTrackToExistingPlaylist(long playlistId, long trackId);
        Task RemoveTrackFromPlaylist(long trackId, long playlistId);
        Task<ClientModels.Playlist> GetPlaylistById(long playlistId, string currentUserId);
    }
}

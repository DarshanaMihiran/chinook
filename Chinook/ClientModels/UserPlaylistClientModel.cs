using Chinook.Models;

namespace Chinook.ClientModels
{
    public class UserPlaylistClientModel
    {
        public string? UserId { get; set; }
        public long PlaylistId { get; set; }
        public ChinookUser? User { get; set; }
        public Playlist? Playlist { get; set; }
    }
}

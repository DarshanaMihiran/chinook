using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Helpers
{
    public static class MessageGenerator
    {
        public static string GenerateAssignTrackInfoMessage(PlaylistTrack track, string playlistName)
        {
            return $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} added to playlist {playlistName}.";
        }

        public static string GenerateRemoveTrackInfoMessage(PlaylistTrack track, string playlistName)
        {
            return $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} removed from playlist {playlistName}.";
        }

        public static string GenerateNewPlaylistInfoMessage(PlaylistTrack track, string playlistName, long playlistId)
        {
            return $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} added to playlist <a href='/playlist/{playlistId}'>{playlistName}</a>.";
        }
    }
}

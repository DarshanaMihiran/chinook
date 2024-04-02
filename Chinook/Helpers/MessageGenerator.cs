using Chinook.ClientModels;

namespace Chinook.Helpers
{
    public static class MessageGenerator
    {
        public static string GenerateAssignTrackInfoMessage(PlaylistTrack track, string playlistName)
        {
            return GenerateTrackMessage("added to", track, playlistName);
        }

        public static string GenerateRemoveTrackInfoMessage(PlaylistTrack track, string playlistName)
        {
            return GenerateTrackMessage("removed from", track, playlistName);
        }

        public static string GenerateNewPlaylistInfoMessage(PlaylistTrack track, string playlistName)
        {
            return GenerateTrackMessage("added to", track, playlistName);
        }

        private static string GenerateTrackMessage(string action, PlaylistTrack track, string playlistName)
        {
            return $"Track {track.ArtistName} - {track.AlbumTitle} - {track.TrackName} {action} playlist {playlistName}.";
        }
    }
}

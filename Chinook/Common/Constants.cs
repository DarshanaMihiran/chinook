using Microsoft.AspNetCore.Components.Authorization;

namespace Chinook.Common
{
    public static class Constants
    {
        #region Messages
        public const string UserNotFoundMessage = "User not found.";
        public const string UserIdClaimNotFoundMessage = "UserId Claim not found.";
        public const string AuthenticationStateNotFoundMessage = "Authentication State not found.";
        public const string TrackNotFoundMessage = "Track not found.";
        public const string PlaylistInputAndSelectMessage = "Please select an existing playlist or enter a name for the new playlist.";
        public const string PlaylistCreationErrorMessage = "Error occurred while creating new playlist ({0}): {1}.";
        public const string PlaylistNotFoundMessage = "Playlist not found.";
        public const string PlaylistTracksAdditionErrorMessage = "Error occurred while adding new tracks to playlist ({0}): {1}.";
        public const string MyFavoriteTracksMessage = "My Favorite Tracks";
        public const string FavoritesNotFoundMessage = "Favorites not found.";
        public const string TheTrackExistsMessage = "The track is already in the playlist.";
        public const string PlaylistExistsMessage = "Playlist Exists";
        #endregion

        #region Strings
        public const string Favorites = "Favorites";
        public const string LineBrake = "<br />";
        #endregion
    }
}

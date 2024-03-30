using AutoMapper;
using global::Chinook.ClientModels;
using global::Chinook.Helpers;
using global::Chinook.Models;
using global::Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ChinookContext _context;
        public event EventHandler? PlaylistAdded;

        public PlaylistService(ChinookContext context)
        {
            _context = context;
        }

        #region Public Methods
        /// <summary>
        /// Get list of tracks by artist id asynchronously
        /// </summary>
        /// <param name="artistId">The Artist Id</param>
        /// <param name="currentUserId">Logged User Id</param>
        /// <returns>List of PlaylistTrack</returns>
        public async Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long artistId, string currentUserId)
        {
            return await _context.Tracks
                .Where(t => t.Album.ArtistId == artistId)
                .Include(t => t.Album)
                .Select(t => new PlaylistTrack
                {
                    AlbumTitle = t.Album == null ? "-" : t.Album.Title,
                    TrackId = t.TrackId,
                    TrackName = t.Name,
                    IsFavorite = t.Playlists
                        .Any(p => p.UserPlaylists
                            .Any(up => up.UserId == currentUserId && up.Playlist.Name == "Favorites"))
                })
                .ToListAsync();
        }

        /// <summary>
        /// Remove tracks from favorites asynchronously
        /// </summary>
        /// <param name="trackId">The track id</param>
        /// <param name="currentUserId">Logged User Id</param>
        /// <exception cref="ArgumentException"></exception>
        public async Task UnfavoriteTrackAsync(long trackId, string currentUserId)
        {
            var favoritePlaylist = await GetFavoritePlaylist(currentUserId) ?? throw new ArgumentException("Favorites not found");
            await RemoveTrackFromPlaylist(trackId, favoritePlaylist.PlaylistId);
        }

        /// <summary>
        /// Add tracks to the favorites asynchronously
        /// </summary>
        /// <param name="trackId">The track id</param>
        /// <param name="currentUserId">Logged User Id</param>
        public async Task FavoriteTrackAsync(long trackId, string currentUserId)
        {
            var track = await GetTrackById(trackId);
            var favoritePlaylist = await GetFavoritePlaylist(currentUserId);

            if (favoritePlaylist != null)
            {
                await AddTrackToPlaylist(favoritePlaylist.PlaylistId, trackId);
            }
            else
            {
                var playList = await CreatePlaylistAsync("Favorites", track, currentUserId);
                await CreateUserPlaylistAsync(playList, currentUserId);
            }
            OnPlaylistAdded();
        }

        /// <summary>
        /// Get playlists by userId asynchronously
        /// </summary>
        /// <param name="currentUserId">Logged User Id</param>
        /// <returns>The list of Playlist</returns>
        public async Task<List<ClientModels.Playlist>> GetPlaylistsByUserIdAsync(string currentUserId)
        {
            return await _context.UserPlaylists
                .Include(x => x.Playlist)
                .Where(up => up.UserId == currentUserId)
                .Select(t => new ClientModels.Playlist()
                {
                    PlaylistId = t.Playlist.PlaylistId,
                    Name = t.Playlist.Name
                }).ToListAsync();
        }

        /// <summary>
        /// Add track to the newly created playlist asynchronously
        /// </summary>
        /// <param name="trackId">Track Id</param>
        /// <param name="PlaylistName">Name of the playlist</param>
        /// <param name="currentUserId">Logged User Id</param>
        /// <returns>long value</returns>
        public async Task<long> AddTrackToThePlaylist(long trackId, string PlaylistName, string currentUserId)
        {
            var track = await GetTrackById(trackId);
            var playList = await CreatePlaylistAsync(PlaylistName, track, currentUserId);
            await CreateUserPlaylistAsync(playList, currentUserId);

            OnPlaylistAdded();

            return playList.PlaylistId;
        }

        /// <summary>
        /// Add track to playlist asynchronously
        /// </summary>
        /// <param name="playlistId">Id of the playlist</param>
        /// <param name="track">Track object</param>
        /// <exception cref="ArgumentException"></exception>
        public async Task AddTrackToPlaylist(long playlistId, long trackId)
        {
            var track = await GetTrackById(trackId);
            var playlist = await _context.Playlists.FirstOrDefaultAsync(x => x.PlaylistId == playlistId) ?? throw new ArgumentException("Playlist not found");
            playlist.Tracks.Add(track);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove track from favorites asynchronously
        /// </summary>
        /// <param name="track">Track object</param>
        /// <param name="playlistId">Id of the playlist</param>
        /// <exception cref="ArgumentException"></exception>
        public async Task RemoveTrackFromPlaylist(long trackId, long playlistId)
        {
            var track = await _context.Tracks.FindAsync(trackId) ?? throw new ArgumentException("Track not found");
            var playlist = await _context.Playlists
                .Include(x => x.Tracks)
                .FirstOrDefaultAsync(x => x.PlaylistId == playlistId) ?? throw new ArgumentException("Playlist not found");
            playlist.Tracks.Remove(track);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get playlist by playlist id
        /// </summary>
        /// <param name="currentUserId">Logged user id</param>
        /// <param name="playlistId">Id of the playlist</param>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ClientModels.Playlist> GetPlaylistById(long playlistId, string currentUserId)
        {
            return await _context.Playlists
                .Include(p => p.Tracks)
                .ThenInclude(t => t.Album)
                .ThenInclude(a => a.Artist)
                .Where(p => p.PlaylistId == playlistId)
                .Select(p => new ClientModels.Playlist
                {
                    Name = p.Name,
                    Tracks = p.Tracks.Select(t => new ClientModels.PlaylistTrack
                    {
                        AlbumTitle = t.Album.Title,
                        ArtistName = t.Album.Artist.Name,
                        TrackId = t.TrackId,
                        TrackName = t.Name,
                        IsFavorite = t.Playlists
                            .Any(pl => pl.UserPlaylists
                                .Any(up => up.UserId == currentUserId && up.Playlist.Name == "Favorites"))
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get favorite playlist by logged user id asynchronously
        /// </summary>
        /// <param name="currentUserId">Logged User Id</param>
        /// <returns></returns>
        private async Task<UserPlaylist?> GetFavoritePlaylist(string currentUserId)
        {
            return await _context.UserPlaylists
                .Include(up => up.Playlist)
                .FirstOrDefaultAsync(up => up.UserId == currentUserId && up.Playlist.Name == "Favorites");
        }

        /// <summary>
        /// Get track by track id asynchronously
        /// </summary>
        /// <param name="trackId">Track Id</param>
        /// <returns>Track object</returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<Track> GetTrackById(long trackId)
        {
            return await _context.Tracks.FindAsync(trackId) ?? throw new ArgumentException("Track not found");
        }

        /// <summary>
        /// Create playlist asynchronously
        /// </summary>
        /// <param name="PlaylistName">Name of the playlist</param>
        /// <param name="track">Track object</param>
        /// <returns>Playlist object</returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<Models.Playlist> CreatePlaylistAsync(string PlaylistName, Track track, string currentUserId)
        {
            if (await _context.Playlists
                    .Include(y => y.UserPlaylists)
                    .AnyAsync(x => x.Name.ToUpper() == PlaylistName.ToUpper() &&
                                                    x.UserPlaylists.Any(x => x.UserId == currentUserId)))
                throw new ArgumentException("Playlist Exists");

            var playList = new Models.Playlist()
            {
                PlaylistId = IdGenerator.GenerateUniqueId(),
                Name = PlaylistName,
                Tracks = new List<Track> { track },
            };

            _context.Playlists.Add(playList);
            await _context.SaveChangesAsync();

            OnPlaylistAdded();

            return playList;
        }

        /// <summary>
        /// Create user playlist asynchronously
        /// </summary>
        /// <param name="playlist">Playlist object</param>
        /// <param name="currentUserId">Logged user id</param>
        private async Task CreateUserPlaylistAsync(Models.Playlist playlist, string currentUserId)
        {
            var userPlayList = new UserPlaylist
            {
                Playlist = playlist,
                PlaylistId = playlist.PlaylistId,
                UserId = currentUserId
            };
            _context.UserPlaylists.Add(userPlayList);
            await _context.SaveChangesAsync();
        }
        #endregion
        
        protected virtual void OnPlaylistAdded()
        {
            PlaylistAdded?.Invoke(this, EventArgs.Empty);
        }

    }
}


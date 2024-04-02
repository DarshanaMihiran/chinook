using AutoMapper;
using Chinook.ClientModels;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ChinookContext _context;
        private readonly IMapper _mapper;

        public ArtistService(ChinookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Public Methods
        /// <summary>
        /// Get artists information asynchronously
        /// </summary>
        /// <returns>List of ArtistClientModel</returns>
        public async Task<List<Artist>> GetArtistsAsync()
        {
            var artists = await _context.Artists
                    .Include(x => x.Albums)
                    .ToListAsync();
            return _mapper.Map<List<Artist>>(artists);
        }

        /// <summary>
        /// Get artists information for search asynchronously
        /// </summary>
        /// <returns>List of ArtistClientModel</returns>
        public async Task<List<Artist>> GetArtistsBySearchAsync(string searchTerm)
        {
            var normalizedSearchTerm = searchTerm.ToUpper();
            var artists = await _context.Artists
                            .Include(artist => artist.Albums)
                            .Where(artist => artist.Name.ToUpper()
                            .Contains(normalizedSearchTerm))
                            .ToListAsync();
            return _mapper.Map<List<Artist>>(artists);
        }

        /// <summary>
        /// Get artist information by artist id asynchronously
        /// </summary>
        /// <returns>ArtistClientModel object</returns>
        public async Task<Artist> GetArtistByIdAsync(long artistId)
        {
            var artist = await _context.Artists.SingleOrDefaultAsync(a => a.ArtistId == artistId);
            return _mapper.Map<Artist>(artist);
        }

        /// <summary>
        /// Get albums information by artist id asynchronously
        /// </summary>
        /// <returns>List of AlbumClientModel</returns>
        public async Task<List<Album>> GetAlbumsByArtistIdAsync(int artistId)
        {
            var albums = await _context.Albums
                            .Where(a => a.ArtistId == artistId)
                            .ToListAsync();
            return _mapper.Map<List<Album>>(albums);
        }
        #endregion
    }
}
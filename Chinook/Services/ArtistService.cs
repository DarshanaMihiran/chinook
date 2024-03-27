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
        public async Task<List<ArtistClientModel>> GetArtistsAsync()
        {
            var artists = await _context.Artists.Include(x => x.Albums).ToListAsync();
            return _mapper.Map<List<ArtistClientModel>>(artists);
        }

        /// <summary>
        /// Get artists information for search asynchronously
        /// </summary>
        /// <returns>List of ArtistClientModel</returns>
        public async Task<List<ArtistClientModel>> GetArtistsBySearchAsync(string searchTerm)
        {
            var artists = await _context.Artists.Include(x => x.Albums).Where(a => a.Name.ToUpper().Contains(searchTerm.ToUpper())).ToListAsync();
            return _mapper.Map<List<ArtistClientModel>>(artists);
        }

        /// <summary>
        /// Get artist information by artist id asynchronously
        /// </summary>
        /// <returns>ArtistClientModel object</returns>
        public async Task<ArtistClientModel> GetArtistByIdAsync(long artistId)
        {
            var artist = await _context.Artists.SingleOrDefaultAsync(a => a.ArtistId == artistId);
            return _mapper.Map<ArtistClientModel>(artist);
        }

        /// <summary>
        /// Get albums information by artist id asynchronously
        /// </summary>
        /// <returns>List of AlbumClientModel</returns>
        public async Task<List<AlbumClientModel>> GetAlbumsByArtistIdAsync(int artistId)
        {
            var albums = await _context.Albums.Where(a => a.ArtistId == artistId).ToListAsync();
            return _mapper.Map<List<AlbumClientModel>>(albums);
        }
        #endregion
    }
}
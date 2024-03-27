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
        public async Task<List<ArtistClientModel>> GetArtistsAsync()
        {
            var artists = await _context.Artists.Include(x => x.Albums).ToListAsync();
            return _mapper.Map<List<ArtistClientModel>>(artists);
        }

        public async Task<List<ArtistClientModel>> GetArtistsBySearchAsync(string searchTerm)
        {
            var artists = await _context.Artists.Include(x => x.Albums).Where(a => a.Name.ToUpper().Contains(searchTerm.ToUpper())).ToListAsync();
            return _mapper.Map<List<ArtistClientModel>>(artists);
        }

        public async Task<ArtistClientModel> GetArtistByIdAsync(long artistId)
        {
            var artist = await _context.Artists.SingleOrDefaultAsync(a => a.ArtistId == artistId);
            return _mapper.Map<ArtistClientModel>(artist);
        }

        public async Task<List<AlbumClientModel>> GetAlbumsByArtistIdAsync(int artistId)
        {
            var albums = await _context.Albums.Where(a => a.ArtistId == artistId).ToListAsync();
            return _mapper.Map<List<AlbumClientModel>>(albums);
        }
        #endregion
    }
}
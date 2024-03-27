using AutoMapper;
using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Artist, ArtistClientModel>();
            CreateMap<Album, AlbumClientModel>();
            CreateMap<UserPlaylist, UserPlaylistClientModel>();
        }
    }
}

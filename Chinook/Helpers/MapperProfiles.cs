using AutoMapper;
using Chinook.ClientModels;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Artist, ArtistClientModel>();
            CreateMap<Album, AlbumClientModel>();
            CreateMap<UserPlaylist, UserPlaylistClientModel>();
            CreateMap<Track, TrackClientModel>();
            CreateMap<Models.Playlist, ClientModels.Playlist>();
            CreateMap<Customer, CustomerClientModel>();
            CreateMap<Employee, EmployeeClientModel>();
            CreateMap<Genre, GenreClientModel>();
            CreateMap<Invoice, InvoiceClientModel>();
            CreateMap<InvoiceLine, InvoiceLineClientModel>();
            CreateMap<MediaType, MediaTypeClientModel>();
            CreateMap<Track, PlaylistTrack>();

        }
    }
}

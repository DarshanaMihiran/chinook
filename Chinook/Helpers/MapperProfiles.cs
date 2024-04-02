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
            CreateMap<Models.Artist, ClientModels.Artist>().ReverseMap();
            CreateMap<Models.Album, ClientModels.Album>().ReverseMap();
            CreateMap<Models.UserPlaylist, ClientModels.UserPlaylist>().ReverseMap();
            CreateMap<Models.Track, ClientModels.Track>().ReverseMap();
            CreateMap<Models.Playlist, ClientModels.Playlist>().ReverseMap();
            CreateMap<Models.Customer, ClientModels.Customer>().ReverseMap();
            CreateMap<Models.Employee, ClientModels.Employee>().ReverseMap();
            CreateMap<Models.Genre, ClientModels.Genre>().ReverseMap();
            CreateMap<Models.Invoice, ClientModels.Invoice>().ReverseMap();
            CreateMap<Models.InvoiceLine, ClientModels.InvoiceLine>().ReverseMap();
            CreateMap<Models.MediaType, ClientModels.MediaType>().ReverseMap();
            CreateMap<Models.Track, PlaylistTrack>().ReverseMap();

        }
    }
}

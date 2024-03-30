using Chinook.Services.Interfaces;
using Chinook.Services;
using Chinook.Areas.Identity;
using Chinook.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Chinook.Helpers
{
    public static class ServiceConfigurator
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ChinookUser>>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
        }
    }
}

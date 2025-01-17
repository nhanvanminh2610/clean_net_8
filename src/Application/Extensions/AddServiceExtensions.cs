using Application.Services.Concrete;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class AddServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = services.BuildServiceProvider();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}

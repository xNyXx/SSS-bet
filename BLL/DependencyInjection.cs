using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BLL.Services;
using BLL.Services.PassportService;

namespace BLL
{
    public static class Extentions
    {
        public static IServiceCollection ConfigureBusinessLogic(this IServiceCollection services,
                    IConfiguration configuration)
        {
            services.AddScoped<PassportService>();
            services.AddScoped<ProfileService>();

            return services;
        }
    }
}

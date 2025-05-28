using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taxio.DataAccess.Services;

namespace Taxio.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
        {
            // Database
            services.AddDbContext<TaxioDbContext>(options =>
            {
                var connectionString = config.GetConnectionString("DefaultConnection");

                options.UseSqlServer(connectionString);
            });

            // Services
            services.AddScoped<IVehicleService, VehicleService>();

            return services;
        }
    }
}

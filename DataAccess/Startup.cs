using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repositories;
using Microsoft.Extensions.Options;

namespace DataAccess
{
    public static class Startup
    {
        public static void ConfigureDataAccessRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IUserHistoryRepository, UserHistoryRepository>();
            services.AddTransient<IPhysicalLocationRepository, PhysicalLocationRepository>();
            services.AddTransient<IApplicationLoggingRepository, ApplicationLoggingRepository>();

#if DEBUG
            services.AddDbContext<ApplicationDbContext>(options =>
                options.EnableSensitiveDataLogging());
#else       
            services.AddDbContext<ApplicationDbContext>();
#endif

        }
    }
}

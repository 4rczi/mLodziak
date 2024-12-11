using DataAccess.Repositories;
using DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedServices
{
    public static class Startup
    {
        public static void ConfigureSharedServicesServices(this IServiceCollection services)
        {
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPhysicalLocationService, PhysicalLocationService>();
            services.AddTransient<IUserHistoryService, UserHistoryService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
        
}

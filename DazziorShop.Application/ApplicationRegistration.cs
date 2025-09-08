using DazziorShop.Application.Common;
using DazziorShop.Application.Services;
using DazziorShop.Application.Services.Interface;
using DazziorShop.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DazziorShop.Application
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });


            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}

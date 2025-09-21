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
            //services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IPaginationService<,>), typeof(PaginationService<,>));



            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IBrandService, BrandService>();

            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}

using DazziorShop.Domain.Model;
using DazziorShop.Infrastructure.Dbcontexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DazziorShop.Infrastructure.Common
{
    public class SeedData
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "CUSTOMER", NormalizedName = "CUSTOMER" },
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedDataAsync(ApplicationDbContext _dbContext)
        {
            if (!_dbContext.Brand.Any())
            {
                await _dbContext.AddRangeAsync(
                    
                    new Brand
                    { 
                        Name = "Samsung",
                        EstablishedYear = 1988
                    },
                    new Brand
                    { 
                        Name = "HP",
                        EstablishedYear = 1999
                    },
                    new Brand
                    { 
                        Name = "Apple",
                        EstablishedYear = 1984
                    },
                    new Brand
                    { 
                        Name = "Xiami",
                        EstablishedYear = 1956
                    },
                    new Brand
                    { 
                        Name = "Hyundai",
                        EstablishedYear = 1944
                    },
                    new Brand
                    { 
                        Name = "BMV",
                        EstablishedYear = 1959
                    }
                    );

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

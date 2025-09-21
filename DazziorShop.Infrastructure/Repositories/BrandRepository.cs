using DazziorShop.Domain.Contracts;
using DazziorShop.Domain.Model;
using DazziorShop.Infrastructure.Dbcontexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DazziorShop.Infrastructure.Repositories
{
    public class BrandRepository : GenericRepository<Brand> , IBrandRepository
    {
        public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task UpdateAsync(Brand brand)
        {
            _dbContext.Update(brand);
            await _dbContext.SaveChangesAsync();
        }
    }
}

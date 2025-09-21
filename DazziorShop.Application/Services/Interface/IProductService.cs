using DazziorShop.Application.DTO.Product;
using DazziorShop.Application.InputModels;
using DazziorShop.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DazziorShop.Application.Services.Interface
{
    public interface IProductService
    {
        Task<PaginationVM<ProductDto>> GetPagination(PaginationInputModel pagination);
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IEnumerable<ProductDto>> GetAllFilterAsync(int? categoryId,int? brandId);
        Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
        Task UpdateAsync(UpdateProductDto updateProductDto);
        Task DeleteAsync(int id);
    }
}

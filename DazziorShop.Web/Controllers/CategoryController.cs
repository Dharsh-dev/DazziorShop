using DazziorShop.Application.DTO.Category;
using DazziorShop.Application.Services;
using DazziorShop.Application.Services.Interface;
using DazziorShop.Domain.Contracts;
using DazziorShop.Domain.Model;
using DazziorShop.Infrastructure.Dbcontexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace DazziorShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult> Get(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if(category == null)
            {
                return NotFound($"Id no {id} for Category is not found");
            }
            return Ok(category);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var entity = await _categoryService.CreateAsync(dto);
            return Ok();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateCategoryDto dto)
        {
            await _categoryService.UpdateAsync(dto);
            return NoContent();
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if(category == null)
            {
                return BadRequest();
            }
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }




    }
}

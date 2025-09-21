using Asp.Versioning;
using DazziorShop.Application.ApplicationConstants;
using DazziorShop.Application.Common;
using DazziorShop.Application.DTO.Category;
using DazziorShop.Application.Services;
using DazziorShop.Application.Services.Interface;
using DazziorShop.Domain.Contracts;
using DazziorShop.Domain.Model;
using DazziorShop.Infrastructure.Dbcontexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using System.Net;
using System.Threading.Tasks;

namespace DazziorShop.Web.Controllers.v1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        protected APIResponse _response;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _response = new APIResponse();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {

            try
            {
                var categories = await _categoryService.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = categories;
            }
            catch(Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.SystemError);
                //_response.AddError(ex.Message.ToString());
            }
            return Ok(_response);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult<APIResponse>> Get(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);

                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.RecordNotFound;

                    return Ok(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = category;

            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.SystemError);
            }

            return Ok(_response);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Create([FromBody] CreateCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.CreateOperationFailed;
                    _response.AddError(ModelState.ToString());
                    return Ok(_response);
                }
                var entity = await _categoryService.CreateAsync(dto);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommonMessage.CreateOperationsuccess;
                _response.Result = entity;
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommonMessage.CreateOperationFailed;
                _response.AddError(CommonMessage.SystemError);
            }
            return Ok(_response);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<ActionResult<APIResponse>> Update([FromBody] UpdateCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.UpdateOperationFailed;
                    _response.AddError(ModelState.ToString());
                    return Ok(_response);
                }
                var category = await _categoryService.GetByIdAsync(dto.Id);

                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.UpdateOperationFailed;
                    return Ok(_response);
                }
                await _categoryService.UpdateAsync(dto);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommonMessage.UpdateOperationsuccess;
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommonMessage.UpdateOperationFailed;
                _response.AddError(CommonMessage.SystemError);
            }
            return Ok(_response);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.DeleteOperationFailed;
                    return Ok(_response);

                }
                var category = await _categoryService.GetByIdAsync(id);

                if (category == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.DeleteOperationFailed;
                    return Ok(_response);

                }
                await _categoryService.DeleteAsync(id);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommonMessage.DeleteOperationsuccess;
                
            }
            catch
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommonMessage.DeleteOperationFailed;
                _response.AddError(CommonMessage.SystemError);
            }

            return Ok(_response);
        }




    }
}

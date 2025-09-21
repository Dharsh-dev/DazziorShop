using Asp.Versioning;
using DazziorShop.Application.ApplicationConstants;
using DazziorShop.Application.Common;
using DazziorShop.Application.DTO.Product;
using DazziorShop.Application.InputModels;
using DazziorShop.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DazziorShop.Web.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        protected APIResponse _response;

        public ProductController(IProductService productService)
        {
            _productService = productService;
            _response = new APIResponse();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {

            try
            {
                var products = await _productService.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = products;
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.SystemError);
                //_response.AddError(ex.Message.ToString());
            }
            return Ok(_response);
        }

        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Route("GetPagination")]
        public async Task<ActionResult<APIResponse>> GetPagination(PaginationInputModel pagination)
        {

            try
            {
                var products = await _productService.GetPagination(pagination);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = products;
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.SystemError);
                //_response.AddError(ex.Message.ToString());
            }
            return Ok(_response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("Filter")]
        public async Task<ActionResult<APIResponse>> GetAllAsync(int? categoryId, int? brandId)
        {

            try
            {
                var products = await _productService.GetAllFilterAsync(categoryId,brandId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = products;
            }
            catch (Exception)
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
                var product = await _productService.GetByIdAsync(id);

                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.RecordNotFound;

                    return Ok(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = product;

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
        public async Task<ActionResult<APIResponse>> Create([FromBody] CreateProductDto dto)
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
                var entity = await _productService.CreateAsync(dto);

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
        public async Task<ActionResult<APIResponse>> Update([FromBody] UpdateProductDto dto)
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
                var product = await _productService.GetByIdAsync(dto.Id);

                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.UpdateOperationFailed;
                    return Ok(_response);
                }
                await _productService.UpdateAsync(dto);

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
                var product = await _productService.GetByIdAsync(id);

                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.DeleteOperationFailed;
                    return Ok(_response);

                }
                await _productService.DeleteAsync(id);
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

using Asp.Versioning;
using DazziorShop.Application.ApplicationConstants;
using DazziorShop.Application.Common;
using DazziorShop.Application.DTO.Brand;
using DazziorShop.Application.Exceptions;
using DazziorShop.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DazziorShop.Web.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/BrandController")]
    [ApiVersion("1.0")]
    public class Brandv1Controller : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping() => Ok("Hello from API version 1.0");

        private readonly IBrandService _brandService;
        protected APIResponse _response;

        public Brandv1Controller(IBrandService brandService)
        {
            _brandService = brandService;
            _response = new APIResponse();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = "Default")]
        [HttpGet("all")]
        public async Task<ActionResult<APIResponse>> Get()
        {

            try
            {
                var brands = await _brandService.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = brands;
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
        [ResponseCache(CacheProfileName = "Default")]
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult<APIResponse>> Get(int id)
        {
            try
            {
                var brand = await _brandService.GetByIdAsync(id);

                if (brand == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.RecordNotFound;

                    return Ok(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = brand;

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
        public async Task<ActionResult<APIResponse>> Create([FromBody] CreateBrandDto dto)
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
                var entity = await _brandService.CreateAsync(dto);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommonMessage.CreateOperationsuccess;
                _response.Result = entity;
            }

            catch (BadRequestException ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommonMessage.CreateOperationFailed;
                _response.AddWarning(ex.Message);
                _response.AddError(CommonMessage.SystemError);
                _response.Result = ex.ValidationsErrors;

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
        public async Task<ActionResult<APIResponse>> Update([FromBody] UpdateBrandDto dto)
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
                var brand = await _brandService.GetByIdAsync(dto.Id);

                if (brand == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.UpdateOperationFailed;
                    return Ok(_response);
                }
                await _brandService.UpdateAsync(dto);

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
                var brand = await _brandService.GetByIdAsync(id);

                if (brand == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommonMessage.DeleteOperationFailed;
                    return Ok(_response);

                }
                await _brandService.DeleteAsync(id);
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

using Asp.Versioning;
using DazziorShop.Application.ApplicationConstants;
using DazziorShop.Application.Common;
using DazziorShop.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DazziorShop.Web.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/brand")]
    [ApiVersion("2.0")]
    public class Brandv2Controller : ControllerBase
    {
        private readonly IBrandService _brandService;
        protected APIResponse _response;

        public Brandv2Controller(IBrandService brandService)
        {
            _brandService = brandService;
            _response = new APIResponse();
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("Hello from API version 2.0");

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
            catch
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.SystemError);
            }
            return Ok(_response);
        }

        [HttpGet("Details/{id}")]
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
            catch
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.SystemError);
            }
            return Ok(_response);
        }
    }
}

using Asp.Versioning;
using Azure;
using DazziorShop.Application.ApplicationConstants;
using DazziorShop.Application.Common;
using DazziorShop.Application.InputModels;
using DazziorShop.Application.Services;
using DazziorShop.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;

namespace DazziorShop.Web.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected APIResponse _response;
        public UserController(IAuthService authService)
        {
            _authService = authService;
            _response = new APIResponse();
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<APIResponse>> Register(Register register)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    _response.AddError(ModelState.ToString());
                    _response.AddWarning(CommonMessage.RegistrationFailed);
                    return _response;
                }

                var result = await _authService.Register(register);

                //_response.IsSuccess = true;
                //_response.StatusCode = HttpStatusCode.Created;
                //_response.DisplayMessage = CommonMessage.RegistrationSuccess;
                //_response.Result = result;

                if (result.Any()) // Error irundha (duplicate, weak password, etc.)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommonMessage.RegistrationFailed; // e.g., "User already exists"
                    _response.Result = result;
                }
                else // Success
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.Created;
                    _response.DisplayMessage = CommonMessage.RegistrationSuccess;
                    _response.Result = "User Created Successfully";
                }

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
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<APIResponse>> Login(Login login)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    _response.AddError(ModelState.ToString());
                    _response.AddWarning(CommonMessage.RegistrationFailed);
                    return _response;
                }

                var result = await _authService.Login(login);

                if(result is string)
                { 
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.DisplayMessage = CommonMessage.LoginFailed;
                    _response.Result = result;
                    return _response;
                }

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.DisplayMessage = CommonMessage.Loginuccess;
                _response.Result = result;

            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommonMessage.SystemError);
                //_response.AddError(ex.Message.ToString());
            }
            return Ok(_response);
        }
    }
}

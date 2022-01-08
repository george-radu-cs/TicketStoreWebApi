using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketStore.Entities;
using TicketStore.Managers;
using TicketStore.Models;
using TicketStore.Utils;

namespace TicketStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserModel model)
        {
            try
            {
                var response = await _authenticationManager.SignUp(model);
                if (response.Success) // the user was created successfully
                {
                    return Created("success", JsonConvert.SerializeObject("User created successfully"));
                }

                return response.ErrorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"SignUp failed. Error message: {response.ErrorMessage}")),
                    // for any other errors return 500 
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"SignUp failed. Error message: {e.Message}"));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            try
            {
                // try to login the user and send back the user's token
                var response = await _authenticationManager.Login(model);
                if (response.Record != null)
                {
                    return Ok(response.Record);
                }

                return response.ErrorType switch
                {
                    ErrorTypes.UserFault => BadRequest(JsonConvert.SerializeObject($"Login failed. Error message: {response.ErrorMessage}")),
                    // for any other errors return 500
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"SignUp failed. Error message: {e.Message}"));
            }
        }

        [HttpGet("user")]
        [Authorize(Policy = AuthorizationRoles.Anyone)]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity)
                {
                    return BadRequest(JsonConvert.SerializeObject("JWT error"));
                }

                var emailAddressClaim = claimsIdentity.Claims
                    .FirstOrDefault(c => c.Type.Contains("emailaddress"));
                if (emailAddressClaim == null)
                {
                    return BadRequest(JsonConvert.SerializeObject("JWT error"));
                }

                // get the current user info
                var response = await _authenticationManager.GetUser(emailAddressClaim.Value);

                if (response.Record != null)
                {
                    return Ok(response.Record);
                }

                return response.ErrorType switch
                {
                    "USER" => BadRequest(JsonConvert.SerializeObject($"Couldn't get the current user info. Error message: {response.ErrorMessage}")),
                    "SERVER" or _ => StatusCode(StatusCodes.Status500InternalServerError),
                };
            }
            catch (Exception e)
            {
                return BadRequest(JsonConvert.SerializeObject($"Couldn't get the current user. Error message: {e.Message}"));
            }
        }
    }
}
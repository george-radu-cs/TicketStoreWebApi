using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                var (success, errorMessage, errorType) = await _authenticationManager.SignUp(model);
                if (success) // the user was created successfully
                {
                    return Created("success", "User created successfully");
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest($"SignUp failed. Error message: {errorMessage}"),
                    // for any other errors return 500 
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest($"SignUp failed. Error message: {e.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            try
            {
                // try to login the user and send back the user's token
                var (token, errorMessage, errorType) = await _authenticationManager.Login(model);
                if (token != null)
                {
                    return Ok(token);
                }

                return errorType switch
                {
                    ErrorTypes.UserFault => BadRequest($"Login failed. Error message: {errorMessage}"),
                    // for any other errors return 500
                    ErrorTypes.ServerFault or _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
            catch (Exception e)
            {
                return BadRequest($"SignUp failed. Error message: {e.Message}");
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketStore.Managers;
using TicketStore.Models;

namespace TicketStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(IAuthenticationManager authenticationManager)
        {
            this._authenticationManager = authenticationManager;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserModel model)
        {
            try
            {
                var success = await _authenticationManager.SignUp(model);
                if (!success)
                {
                    return BadRequest("SignUp failed");
                }

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("SignUp failed");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            try
            {
                var token = await _authenticationManager.Login(model);
                if (token == null)
                {
                    return BadRequest("Login failed");
                }

                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest("Exception caught");
            }
        }
    }
}
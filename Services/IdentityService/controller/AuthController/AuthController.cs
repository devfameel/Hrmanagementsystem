using IdentityService.dtos.Auth;
using IdentityService.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.controller.AuthController
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            try
            {


                if (request == null)
                {
                    return BadRequest(new
                    {
                        message = "Login request Cannot be null"

                    });
                }
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return BadRequest(new
                    {
                        message = "User Name is required"
                    });
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new
                    {
                        message = "Password is required."
                    });
                }

                var result = await _authService.LoginAsync(request);
                if(result == null)
                {
                    return BadRequest(new
                    {
                        massage = "Invalied username or password"
                    });
                }
                return Ok(result);

            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while processing login for user {Username}",
                    request?.Username);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        message = "An unexpected error occurred. Please try again later."
                    });
            }
        }

            

    }
}

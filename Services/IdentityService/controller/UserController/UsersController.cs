using IdentityService.dtos.Users;
using IdentityService.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.controller.UserController
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest req)
        {
            try
            {
                var result = await _userService.CreateAsync(req);

                return Ok(new
                {

                    success = true,
                    data = result
                });

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "User creation failed.");

                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });

            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var Users = await _userService.GetAllUserAsync();

                return Ok(new
                {
                    success = true,
                    data = Users
                });
            }
            catch (Exception ex)

            {
                _logger.LogError(ex, "Error while retrieving users.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Unable to retrieve users."
                });

            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "User not found."
                    });
                }
                return Ok(new
                {
                    success = true,
                    data = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while retrieving user with ID {UserId}.",
                    id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Unable to retrieve user."
                });
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
       int id,
       [FromBody] UpdateUserRequest request)
        {
            try
            {
                var updated = await _userService.UpdateAsync(
                    id,
                    request);

                if (updated == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "User not found."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "User updated successfully.",
                    data = updated
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "User update failed for ID {UserId}.",
                    id);

                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while updating user with ID {UserId}.",
                    id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Unable to update user."
                });
            }
        }

        // DELETE USER
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _userService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "User not found."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "User deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while deleting user with ID {UserId}.",
                    id);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Unable to delete user."
                });
            }

        }
    }

       
        
}
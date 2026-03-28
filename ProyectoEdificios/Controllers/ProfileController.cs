using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoEdificios.Models.DTO.Users;
using ProyectoEdificios.Services.Users;
using System.Security.Claims;

namespace ProyectoEdificios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] UpdateMyProfileDto request,
            CancellationToken cancellationToken)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var authenticatedUserId))
                return Unauthorized(new { message = "No fue posible identificar el usuario autenticado." });

            if (request.Id != authenticatedUserId)
                return BadRequest(new { message = "El id enviado no coincide con el usuario autenticado." });

            var result = await _userService.UpdateMyProfileAsync(
                authenticatedUserId,
                request,
                cancellationToken);

            if (!result.Success && result.Error == "El usuario no existe.")
                return NotFound(new { message = result.Error });

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(result.User);
        }
    }
}

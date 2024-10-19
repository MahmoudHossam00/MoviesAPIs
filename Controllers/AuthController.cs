using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IAuthServices _authServices) : ControllerBase
	{

		[HttpPost("Register")]
		public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authServices.RegisterAsync(model);
			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			return Ok(result);
		}
		[HttpPost("token")]
		public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)

		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _authServices.GetTokenAsync(model);
			if (!result.IsAuthenticated)
			{
				return BadRequest(result.Message);
			}
			return Ok(result);
		}
		[Authorize(Roles = "ADMIN")]
		[HttpPost("addrole")]
		public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)

		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var result = await _authServices.AddRoleAsync(model);
			if (!string.IsNullOrEmpty(result))
			{
				return BadRequest(result);
			}
			return Ok(model);
		}
	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesCore.Models;
using MoviesCore.Services;
using MoviesEF.Repository;

namespace MoviesApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService authService;

		public AuthController(IAuthService _authService)
		{
			authService = _authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await authService.registerAsync(model);

			if (!result.IsAuthanticated)
				return BadRequest(result.Message);

			return Ok(result);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> LoginAsync([FromBody] TokenRequestModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await authService.GetTokenAsync(model);

			if (!result.IsAuthanticated)
				return BadRequest(result.Message);

			return Ok(result);
		}

		[HttpPost("UserToRole")]
		public async Task<IActionResult> UserToRoleAsync([FromBody] AddRoleModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await authService.AddRoleAsync(model);

			if (!string.IsNullOrEmpty(result))
				return BadRequest(result);


			return Ok(model);
		}
	}
}

// Controllers/AccountController.cs
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using task_management_system;

namespace task_management_system
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IConfiguration _configuration;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}

		private string GenerateJwtToken(string username)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
				Expires = DateTime.UtcNow.AddHours(1),
				Audience = _configuration["Jwt:Audience"], // Add the Audience property
				Issuer = _configuration["Jwt:Issuer"], // Add this line
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Fix the unrecognized word "Hmac"
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}




		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				var token = GenerateJwtToken(model.UserName);
				return Ok(new { Token = token });
				// return Ok(model);
			}

			return BadRequest(result.Errors);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

			if (result.Succeeded)
			{
				var token = GenerateJwtToken(model.UserName);
				var user = await _userManager.FindByNameAsync(model.UserName);
				return Ok(new {Token = token, user });
			}

			return BadRequest("Invalid login attempt");
		}

	}


}
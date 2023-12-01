// Controllers/AccountController.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using task_management_system;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;


	public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
	{
		_userManager = userManager;
		_signInManager = signInManager;

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
			return Ok("Registration successful");
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
			var user = await _userManager.FindByNameAsync(model.UserName);
			return Ok(user);
		}

		return BadRequest("Invalid login attempt");
	}
}
using Demo2_WebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Demo2_WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
  [HttpPost("login")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Login(LoginInput model)
  {
    UserInfo? user = GetUser(model.Username, model.Password);
    if (user == null)
    {
      return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Prijava neuspješna.");
    }

    var claims = new List<Claim>
      {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim("FullName", user.FullName)
        };
    if (user.IsAdmin)
    {
      claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
    }

    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

    return NoContent();
  }

  [HttpPost("logout")]
  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return NoContent();
  }

  [Authorize]
  [HttpGet("info")]
  public IEnumerable<ClaimInfo> Info()
  {    
    ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
    IEnumerable<Claim> claims = claimsIdentity?.Claims ?? Enumerable.Empty<Claim>();
    return claims.Select(c => new ClaimInfo(c.Type, c.Value));
  }

  private UserInfo? GetUser(string username, string password)
  {
    if (username == password)
    {
      return Data.Users.FirstOrDefault(u => u.Username == username);
    }
    return null;
  }

}

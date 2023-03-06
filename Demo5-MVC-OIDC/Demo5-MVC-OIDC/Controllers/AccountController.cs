using Auth0.AspNetCore.Authentication;
using Demo5_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Demo5_MVC.Controllers;

public class AccountController : Controller
{
  [HttpPost]
  public async Task Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
  }

  [Authorize]
  public IActionResult Info()
  {    
    ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
    IEnumerable<Claim> claims = claimsIdentity?.Claims ?? Enumerable.Empty<Claim>();
    return View(claims);
  }
}

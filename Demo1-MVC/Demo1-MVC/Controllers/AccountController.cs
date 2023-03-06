using Demo1_MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Demo1_MVC.Controllers;

public class AccountController : Controller
{
  [HttpGet]
  public IActionResult Login()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Login(LoginInput model)
  {
    if (ModelState.IsValid)
    {
      UserInfo? user = GetUser(model.Username, model.Password);
      if (user == null)
      {
        ModelState.AddModelError(string.Empty, "Prijava neuspješna.");
        return View();
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

      if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
      {
        return LocalRedirect(model.ReturnUrl);
      }
      else
      {
        return RedirectToAction("Index", "Home");
      }
    }
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Index", "Home"); 
  }

  [Authorize]
  public IActionResult Info()
  {    
    ClaimsIdentity? claimsIdentity = User.Identity as ClaimsIdentity;
    IEnumerable<Claim> claims = claimsIdentity?.Claims ?? Enumerable.Empty<Claim>();
    return View(claims);
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

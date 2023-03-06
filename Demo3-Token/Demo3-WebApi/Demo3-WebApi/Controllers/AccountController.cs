using Demo3_WebApi.Models;
using Demo3_WebApi.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo3_WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
  private readonly TokenConfig tokenConfig;
  public AccountController(IOptionsSnapshot<TokenConfig> tokenConfigSnapshot)
  {
    tokenConfig = tokenConfigSnapshot.Value;
  }

  [HttpPost("login")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public ActionResult<string> Login(LoginInput model)
  {
    UserInfo? user = GetUser(model.Username, model.Password);
    if (user == null)
    {
      return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: $"Prijava neuspješna.");
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

    JwtSecurityToken token = CreateToken(claims);
    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return tokenString;
  }

  private UserInfo? GetUser(string username, string password)
  {
    if (username == password)
    {
      return Data.Users.FirstOrDefault(u => u.Username == username);
    }
    return null;
  }

  private JwtSecurityToken CreateToken(IEnumerable<Claim> claims)
  {
    var secretKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(tokenConfig.Secret));
    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: tokenConfig.Issuer,
        audience: tokenConfig.Audience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(tokenConfig.AccessExpiration),
        signingCredentials: signinCredentials
    );

    return token;
  }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Demo1_MVC.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class UserInfoController : ControllerBase
{
  private readonly IConfiguration configuration;

  public UserInfoController(IConfiguration configuration) {
    this.configuration = configuration;
  }
  [HttpGet]
  public async Task<object?> GetUserInfo()
  {
    string authorization = Request.Headers.Authorization;
    if (authorization != null && authorization.StartsWith("Bearer "))
    {
      string accessToken = authorization.Substring("Bearer ".Length);
      var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
      var response = await client.GetAsync($"{configuration["AuthSettings:Authority"]}/userinfo");
      response.EnsureSuccessStatusCode();
      dynamic? userInfo = await response.Content.ReadFromJsonAsync<dynamic>();
      return userInfo;
    }
    else {
      return null; 
    }  
  }
}


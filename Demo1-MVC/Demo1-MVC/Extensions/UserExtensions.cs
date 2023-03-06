using System.Security.Claims;

namespace Demo1_MVC.Extensions;

public static class UserExtensions
{
  public static string? FullName(this ClaimsPrincipal principal)
  {
    if (principal.Identity != null && principal.Identity.IsAuthenticated)
    {        
      var claim = principal.FindFirst("FullName");
      string? value = claim?.Value;
      return value;
    }
    else
    {
      return null;
    }
  }
}

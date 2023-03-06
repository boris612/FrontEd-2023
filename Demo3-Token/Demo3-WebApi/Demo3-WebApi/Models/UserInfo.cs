namespace Demo3_WebApi.Models;

public record UserInfo(string Username, string FirstName, string LastName, bool IsAdmin)
{
  public string FullName => $"{FirstName} {LastName}";  
}

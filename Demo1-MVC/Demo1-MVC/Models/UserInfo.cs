namespace Demo1_MVC.Models;

public record UserInfo(string Username, string FirstName, string LastName, bool IsAdmin)
{
  public string FullName => $"{FirstName} {LastName}";  
}

#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Demo3_WebApi.Models
{
  public class LoginInput
  {
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
  }
}

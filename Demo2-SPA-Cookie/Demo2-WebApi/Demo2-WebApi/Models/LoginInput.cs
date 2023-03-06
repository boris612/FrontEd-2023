#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Demo2_WebApi.Models
{
  public class LoginInput
  {
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
  }
}

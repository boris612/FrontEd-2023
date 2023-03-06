#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Demo1_MVC.Models
{
  public class LoginInput
  {
    [Required, Display(Name = "Korisničko ime")]
    public string Username { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Zaporka")]
    public string Password { get; set; }
    
    public string ReturnUrl { get; set; }
  }
}

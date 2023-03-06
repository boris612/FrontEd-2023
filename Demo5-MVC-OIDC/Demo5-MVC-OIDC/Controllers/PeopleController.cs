using Demo5_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo5_MVC.Controllers;

[Authorize]
public class PeopleController : Controller
{
  public IActionResult Index()
  {
    return View();
  }

  public List<PersonDto> Get() => Data.People; 
}

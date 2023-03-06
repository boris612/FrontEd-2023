using Demo1_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo1_MVC.Controllers;

public class PeopleController : Controller
{
  public IActionResult Index()
  {
    return View();
  }

  [Authorize]
  public List<PersonDto> Get() => Data.People; 
}

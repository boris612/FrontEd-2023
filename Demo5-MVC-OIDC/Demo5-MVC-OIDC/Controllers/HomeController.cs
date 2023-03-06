using Microsoft.AspNetCore.Mvc;

namespace Demo5_MVC.Controllers;

public class HomeController : Controller
{
  public IActionResult Index()
  {
    return View();
  }
}

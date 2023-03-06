using Microsoft.AspNetCore.Mvc;

namespace Demo1_MVC.Controllers;

public class HomeController : Controller
{
  public IActionResult Index()
  {
    return View();
  }
}

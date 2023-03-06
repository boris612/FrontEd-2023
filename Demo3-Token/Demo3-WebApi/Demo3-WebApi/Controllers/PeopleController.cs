using Demo3_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo1_MVC.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
  [HttpGet]
  public List<PersonDto> Get() => Data.People; 
}

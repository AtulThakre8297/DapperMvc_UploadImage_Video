using DapperMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperMvc.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
       public IActionResult Authenticate([FromBody] User user) 
        {
            return Ok();
        }
    }
}

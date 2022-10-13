using Microsoft.AspNetCore.Mvc;
using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;

namespace Depozyto.Controllers
{
    public class DashBoardController : Controller
    {
        [Authorize]
        public IActionResult Index(UserModel usr)
        {
           return View();

            
        }
    }
}

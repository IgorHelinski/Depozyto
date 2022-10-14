using Microsoft.AspNetCore.Mvc;
using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;

namespace Depozyto.Controllers
{
    public class DashBoardController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
           return View();
        }

        [Authorize]
        public IActionResult Transaction()
        {
            return View();
        }
        [Authorize]
        public IActionResult Succes()
        {
            return View();
        }
        [Authorize]
        public IActionResult Cancel()
        {
            return View();
        }
        [Authorize]
        public IActionResult History()
        {
            return View();
        }
    }
}

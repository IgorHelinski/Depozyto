using Microsoft.AspNetCore.Mvc;

namespace Depozyto.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Login/Register
        public ActionResult Register()
        {
            return View();
        }
    }
}

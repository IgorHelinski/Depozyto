using Microsoft.AspNetCore.Mvc;

namespace Depozyto.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddAccount()
        {
            return View();
        }
       
    }
}

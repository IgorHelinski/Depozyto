using Microsoft.AspNetCore.Mvc;

namespace Depozyto.Controllers
{
    public class ContractorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddContractor()
        {
            return View();
        }
    }
}

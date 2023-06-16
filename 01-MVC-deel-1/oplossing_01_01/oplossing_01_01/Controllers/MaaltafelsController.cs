using Microsoft.AspNetCore.Mvc;

namespace oplossing_01_01.Controllers
{
    public class MaaltafelsController : Controller
    {
        public IActionResult Index(int aantal)
        {
            ViewData["Aantal"] = aantal;
            return View();
        }

        public IActionResult Details(int tafel)
        {
            ViewData["Tafel"] = tafel;
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace oplossing_01_00.Controllers
{
    public class MVC1Controller : Controller
    {
        public string Index()
        {
            return "Dit is de 'Index' pagina!";
        }

        public string Welkom()
        {
            return "Dit is de 'Welkom' pagina!";
        }

        public IActionResult HelloRazor()
        {
            ViewData["Title"] = "HelloRazor";
            return View();
        }

        public IActionResult HelloViewData()
        {
            return View();
        }
    }
}


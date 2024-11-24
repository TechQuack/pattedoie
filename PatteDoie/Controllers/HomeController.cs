using Microsoft.AspNetCore.Mvc;

namespace PatteDoie.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Blazor()
        {
            return View("_Host");
        }

    }
}

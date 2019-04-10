using Microsoft.AspNetCore.Mvc;

namespace MiniUrl.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Spa()
        {
            return File("~/index.html", "text/html");
        }
    }
}
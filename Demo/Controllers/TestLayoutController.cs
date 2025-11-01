using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class TestLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}

using System.Diagnostics;
using ModelsLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Demo.ViewModels;
using Demo.DAL;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ITIDbContext dbContext;

        public HomeController(ILogger<HomeController> logger,ITIDbContext _dbContext)
        {
            _logger = logger;
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            var model = new HomeDashboardVM
            {
                DepartmentCount = dbContext.Department.Count(),
                StudentCount = dbContext.Students.Count(),
                CourseCount = dbContext.Courses.Count(),
                UserCount = dbContext.Users.Count()
            };
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

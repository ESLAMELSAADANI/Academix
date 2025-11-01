using Demo.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        EntityRepo<Course> courseRepo;
        ICourseRepoExtra courseRepoExtra;

        public CourseController(EntityRepo<Course> _courseRepo, ICourseRepoExtra _courseRepoExtra)
        {
            courseRepo = _courseRepo;
            courseRepoExtra = _courseRepoExtra;
        }

        public async Task<IActionResult> Index()
        {
            var model = await courseRepo.GetAllAsync();
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var course = await courseRepoExtra.DetailsAsync(id.Value);
            if (course == null)
                return NotFound();
            return View(course);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var course = await courseRepo.GetByIdAsync(id.Value);
            if (course == null)
                return NotFound();
            return View(course);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                courseRepo.Update(course);
                await courseRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            var ccourse = await courseRepo.GetByIdAsync(course.Id);
            return View(ccourse);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var course = await courseRepo.GetByIdAsync(id.Value);
            if (course == null)
                return NotFound();
            courseRepo.Delete(course);
            await courseRepo.SaveChangesAsync();
            return RedirectToAction("index");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Course course)
        {
            courseRepo.Delete(course);
            await courseRepo.SaveChangesAsync();
            return RedirectToAction("index");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View(new Course());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(Course course)
        {
            if (ModelState.IsValid)
            {
                await courseRepo.AddAsync(course);
                await courseRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View();
        }
    }
}

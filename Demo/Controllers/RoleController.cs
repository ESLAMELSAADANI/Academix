using Demo.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer;
using System.Data;

namespace Demo.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly EntityRepo<Role> roleRepo;
        private readonly IRoleRepoExtra roleRepoExtra;

        public RoleController(EntityRepo<Role> _roleRepo, IRoleRepoExtra _roleRepoExtra)
        {
            roleRepo = _roleRepo;
            roleRepoExtra = _roleRepoExtra;
        }

        public async Task<IActionResult> Index()
        {
            var model = await roleRepo.GetAllAsync();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var model = await roleRepo.GetByIdAsync(id.Value);
            if (model == null)
                return NotFound();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                roleRepo.Update(role);
                await roleRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            var model = await roleRepo.GetByIdAsync(role.Id);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Role model)
        {
            if (ModelState.IsValid)
            {
                await roleRepo.AddAsync(model);
                await roleRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var role = await roleRepo.GetByIdAsync(id.Value);
            if (role == null)
                return NotFound();
            roleRepo.Delete(role);
            await roleRepo.SaveChangesAsync();
            return RedirectToAction("index");
        }
        [AllowAnonymous]
        public async Task<IActionResult> RoleExist([FromQuery(Name = "RoleName")] string roleName, [FromQuery(Name = "Id")] int id)
        {
            // When adding a new Role (Id == 0)
            if (id == 0)
            {
                bool roleExists = roleRepoExtra.IsRoleExist(roleName);
                return Json(!roleExists); // true if role doesn't exist
            }

            // When editing an existing role
            var existingRole = await roleRepoExtra.GetByNameAsync(roleName);

            if (existingRole == null)
                return Json(true); // Role doesn't exist in DB → valid

            // Role exists but belongs to the same user being edited → valid
            if (existingRole.Id == id)
                return Json(true);

            // Role exists and belongs to another user → invalid
            return Json(false);
        }
    }
}

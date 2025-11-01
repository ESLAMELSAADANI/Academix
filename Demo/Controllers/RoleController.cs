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
            var roleExist = await roleRepoExtra.IsRoleExistAsync(role.RoleName, role.Id);
            if (roleExist)
                ModelState.AddModelError("RoleName", "Role Already Exists!");

            if (ModelState.IsValid)
            {
                //roleRepo.Update(role);//Conflict In Tracking
                var existingRole = await roleRepo.GetByIdAsync(role.Id);
                existingRole.RoleName = role.RoleName;
                roleRepo.Update(existingRole);
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
            var roleExist = await roleRepoExtra.IsRoleExistAsync(model.RoleName, model.Id);
            if (roleExist)
                ModelState.AddModelError("RoleName", "Role Already Exists!");
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
            bool roleExist = await roleRepoExtra.IsRoleExistAsync(roleName, id);
            return Json(!roleExist);
        }
    }
}

using Demo.Repos;
using Demo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer;
using ModelsLayer.Models;
using System.Data;

namespace Demo.Controllers
{
    [Authorize]
    public class userController : Controller
    {
        public readonly EntityRepo<User> userRepo;
        public readonly EntityRepo<Student> studentRepo;
        public readonly EntityRepo<Role> roleRepo;
        public readonly EntityRepo<UserRole> userRoleRepo;

        public readonly IUserRoleRepoExtra userRoleRepoExtra;
        public readonly IUserRepoExtra userExtraRepo;
        public readonly IDepartmentRepoExtra departmentExtraRepo;
        public readonly IStudentRepoExtra studentRepoExtra;


        public readonly IRoleRepoExtra roleRepoExtra;


        public userController(EntityRepo<User> _userRepo, IUserRepoExtra _userExtraRepo, IRoleRepoExtra _roleRepoExtra, IUserRoleRepoExtra _userRoleRepoExtra, EntityRepo<UserRole> _userRoleRepo, EntityRepo<Student> _studentRepo, IStudentRepoExtra _studentRepoExtra, EntityRepo<Role> _roleRepo, IDepartmentRepoExtra _departmentExtraRepo)
        {
            userRepo = _userRepo;
            userExtraRepo = _userExtraRepo;
            roleRepoExtra = _roleRepoExtra;
            userRoleRepoExtra = _userRoleRepoExtra;
            userRoleRepo = _userRoleRepo;
            studentRepo = _studentRepo;
            studentRepoExtra = _studentRepoExtra;
            roleRepo = _roleRepo;
            departmentExtraRepo = _departmentExtraRepo;
        }

        public async Task<IActionResult> Index()
        {
            var model = await userRepo.GetAllAsync();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = await userRepo.GetByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var user = await userExtraRepo.GetUserByIdWithRolesAsync(id.Value);
            if (user == null)
                return NotFound();

            var userRoles = await roleRepoExtra.GetUserRolesAsync(user.Id);
            var model = new UserRoleVM()
            {
                User = user,
                RolesToDelete = userRoles
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            var emailExist = await userExtraRepo.emailExistAsync(model.Email, model.Id);
            if (emailExist)
                ModelState.AddModelError("Email", "Email Exists!");
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<User>();
                // Hash the password before saving it
                model.HashPassword = hasher.HashPassword(model, model.HashPassword);

                userRepo.Update(model);
                //Sync with related Student (if exists)
                var student = await studentRepoExtra.GetStudentByUserIdAsync(model.Id);
                if (student != null)
                {
                    student.Age = model.Age;
                    student.Email = model.Email;
                    student.Password = model.HashPassword;
                    student.Name = model.UserName;
                    studentRepo.Update(student);
                }
                await userRepo.SaveChangesAsync();
                await studentRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            var user = await userRepo.GetByIdAsync(model.Id);
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var model = await userRepo.GetByIdAsync(id.Value);
            if (model == null)
                return NotFound();
            userRepo.Delete(model);
            await userRepo.SaveChangesAsync();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(User model)
        {
            bool emailExist = await userExtraRepo.emailExistAsync(model.Email, model.Id);
            if (emailExist)
                ModelState.AddModelError("Email", "Email Exists!");
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<User>();

                model.HashPassword = hasher.HashPassword(model, model.HashPassword);

                await userRepo.AddAsync(model);
                await userRepo.SaveChangesAsync();
                Role userRole = await roleRepoExtra.GetByNameAsync("user".ToLower());
                await userRoleRepoExtra.AddAsync(model.Id, userRole.Id);
                await userRoleRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewRoles(int? id)
        {
            if (id == null)
                return BadRequest();
            var user = await userExtraRepo.GetUserByIdWithRolesAsync(id.Value);
            if (user == null)
                return NotFound();

            List<Role> rolesToDelete = await roleRepoExtra.GetUserRolesAsync(id.Value);
            List<Role> rolesToAdd = await roleRepoExtra.GetUserAnotherRolesAsync(id.Value);
            UserRoleVM userRoleVM = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            return View(userRoleVM);
        }
        [AllowAnonymous]
        public async Task<IActionResult> EmailExist([FromQuery(Name = "Email")] string email, [FromQuery(Name = "Id")] int id)
        {
            var emailExist = await userExtraRepo.emailExistAsync(email, id);
            return Json(!emailExist);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserRole(int roleId, int userID)
        {
            UserRole userRole = await userRoleRepoExtra.GetAsync(userID, roleId);
            userRoleRepo.Delete(userRole);
            await userRoleRepo.SaveChangesAsync();

            var user = await userRepo.GetByIdAsync(userID);
            var rolesToDelete = await roleRepoExtra.GetUserRolesAsync(userID);
            var rolesToAdd = await roleRepoExtra.GetUserAnotherRolesAsync(userID);

            var model = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            //await HttpContext.SignOutAsync("MyCookieAuth");
            //return RedirectToAction("Login","account");
            return View("ViewRoles", model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserRole(int roleId, int userID)
        {
            UserRole userRole = new UserRole() { UserId = userID, RoleId = roleId };
            await userRoleRepo.AddAsync(userRole);
            await userRoleRepo.SaveChangesAsync();

            var user = await userRepo.GetByIdAsync(userID);
            var rolesToDelete = await roleRepoExtra.GetUserRolesAsync(userID);
            var rolesToAdd = await roleRepoExtra.GetUserAnotherRolesAsync(userID);

            var model = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            //await HttpContext.SignOutAsync("MyCookieAuth");
            //return RedirectToAction("Login","account");
            return View("ViewRoles", model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSelectedRoles(UserRoleVM model)
        {
            if (model.RolesToDeleteIds != null && model.RolesToDeleteIds.Count > 0)
            {
                foreach (var roleId in model.RolesToDeleteIds)
                {
                    var role = await roleRepo.GetByIdAsync(roleId);
                    var firstDept = await departmentExtraRepo.GetFirstDeptAsync();
                    if (role.RoleName == "Student")
                    {
                        Student std = new Student()
                        {
                            Name = model.User.UserName,
                            Email = model.User.Email,
                            Age = model.User.Age,
                            DeptNo = firstDept != null ? firstDept.DeptId : null,
                            Password = model.User.HashPassword,
                        };
                        var student = await studentRepoExtra.GetStudentByEmailAsync(std.Email);
                        studentRepo.Delete(student);
                        await studentRepo.SaveChangesAsync();
                    }
                    var userRole = await userRoleRepoExtra.GetAsync(model.User.Id, roleId);
                    userRoleRepo.Delete(userRole);
                }

                await userRoleRepo.SaveChangesAsync();
                int userId = model.User.Id;
                return RedirectToAction("ViewRoles", new { id = userId });
            }
            ModelState.AddModelError("", "Select Roles To Delete FromThis User!");
            var user = model.User;
            var rolesToDelete = await roleRepoExtra.GetUserRolesAsync(user.Id);
            var rolesToAdd = await roleRepoExtra.GetUserAnotherRolesAsync(user.Id);

            UserRoleVM modell = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            return View("ViewRoles", modell);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSelectedRoles(UserRoleVM model)
        {
            if (model.RolesToAddIds != null && model.RolesToAddIds.Count > 0)
            {
                foreach (var roleId in model.RolesToAddIds)
                {
                    var role = await roleRepo.GetByIdAsync(roleId);
                    var firstDept = await departmentExtraRepo.GetFirstDeptAsync();
                    if (role.RoleName == "Student")
                    {
                        Student std = new Student()
                        {
                            Name = model.User.UserName,
                            Email = model.User.Email,
                            Age = model.User.Age,
                            DeptNo = firstDept != null ? firstDept.DeptId : null,
                            UserId = model.User.Id,
                            Password = model.User.HashPassword,
                        };
                        await studentRepo.AddAsync(std);
                        await studentRepo.SaveChangesAsync();
                    }

                    var userRole = new UserRole() { UserId = model.User.Id, RoleId = roleId };
                    await userRoleRepo.AddAsync(userRole);
                }

                await userRoleRepo.SaveChangesAsync();
                int userId = model.User.Id;
                return RedirectToAction("ViewRoles", new { id = userId });
            }
            ModelState.AddModelError("", "Select Roles To Add FromThis User!");
            var user = model.User;
            var rolesToDelete = await roleRepoExtra.GetUserRolesAsync(user.Id);
            var rolesToAdd = await roleRepoExtra.GetUserAnotherRolesAsync(user.Id);

            UserRoleVM modell = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            return View("ViewRoles", modell);

        }
    }
}

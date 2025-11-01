using Demo.Repos;
using Demo.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ModelsLayer;
using System.Data;
using System.Security.Claims;

namespace Demo.Controllers
{
    public class AccountController : Controller
    {
        public readonly EntityRepo<User> userRepo;
        public readonly EntityRepo<UserRole> userRoleRepo;
        public readonly IUserRepoExtra userExtraRepo;

        public readonly IUserRoleRepoExtra userRoleRepoExtra;

        public readonly IRoleRepoExtra roleRepoExtra;

        public AccountController(EntityRepo<User> _userRepo, IUserRoleRepoExtra _userRoleRepoExtra, IRoleRepoExtra _roleRepoExtra, IUserRepoExtra _userRepoExtra,EntityRepo<UserRole> _userRoleRepo)
        {
            userRepo = _userRepo;
            userRoleRepoExtra = _userRoleRepoExtra;
            roleRepoExtra = _roleRepoExtra;
            userExtraRepo = _userRepoExtra;
            userRoleRepo = _userRoleRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            bool emailExist = await userExtraRepo.emailExistAsync(model.Email,model.Id);
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

                return RedirectToAction("login", "account");
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userExtraRepo.GetUserByEmailPasswordAsync(model.Email, model.Password);
                if (user != null)
                {

                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                    };
                    foreach (var userRole in user.UsersRole)
                    {
                        var role = await roleRepoExtra.GetByIdAsync(userRole.RoleId);
                        claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                    }

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "MyCookieAuth");

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("MyCookieAuth", principal);

                    return RedirectToAction("index", "home");
                }
                //if (model.UserName == "eslam" && model.Password == "123456")
                //{
                //    //if exist in DB
                //    //Store his data in cookies to use them in subrequests after he login to know him as logged in person [Using Claims]

                //    // Step 1: Create claims
                //    List<Claim> claims = new List<Claim>()
                //    {
                //        new Claim(ClaimTypes.Name, model.UserName),
                //        new Claim(ClaimTypes.Email, model.Email),
                //        new Claim(ClaimTypes.Role, "Admin")
                //    };

                //    // Step 2: Create Identity
                //    ClaimsIdentity identity = new ClaimsIdentity(claims, "MyCookieAuth");

                //    // Step 3: Create Principal
                //    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                //    // Step 4: Sign in (store info in cookie)
                //    await HttpContext.SignInAsync("MyCookieAuth", principal);//Cookie that contain claims Created Here in this line.
                //    return RedirectToAction("index", "home");
                //}
            }
            //if  ModelState not valid || user doesn't exist
            ModelState.AddModelError("", "Invalid Credentials!");
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

using Demo.DAL;
using Demo.Repos;
using Demo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;
using ModelsLayer.Models;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        //===== Dependency Injection ========
        EntityRepo<Student> studentRepo;
        EntityRepo<Department> departmentRepo;
        EntityRepo<UserRole> userRoleRepo;
        IRoleRepoExtra roleRepoExtra;
        IUserRoleRepoExtra userRoleRepoExtra;
        IStudentRepoExtra studentRepoExtra;
        IUserRepoExtra userRepoExtra;
        EntityRepo<User> userRepo;

        public StudentController(EntityRepo<Student> _studentRepo, EntityRepo<Department> _departmentRepo, IStudentRepoExtra _studentRepoExtra, IUserRepoExtra _userRepoExtra, EntityRepo<User> _userRepo, IRoleRepoExtra _roleRepoExtra, IUserRoleRepoExtra _userRoleRepoExtra, EntityRepo<UserRole> _userRoleRepo)
        {
            studentRepo = _studentRepo;
            departmentRepo = _departmentRepo;
            studentRepoExtra = _studentRepoExtra;
            userRepoExtra = _userRepoExtra;
            userRepo = _userRepo;
            roleRepoExtra = _roleRepoExtra;
            userRoleRepoExtra = _userRoleRepoExtra;
            userRoleRepo = _userRoleRepo;
        }

        public async Task<IActionResult> Index()
        {
            var students = await studentRepoExtra.GetAllWithDepartmentsAsync();
            return View(students);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = await studentRepoExtra.DetailsAsync(id.Value);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            var depts = await departmentRepo.GetAllAsync();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = new Student(),
                Departments = depts.ToList()
            };
            return View(studentDepartment);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(StudentDepartment studentDepartment)
        {
            var existingUser = await userRepoExtra.GetUserByEmailAsync(studentDepartment.Student.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Student.Email", "Email Exists!");
            }
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = studentDepartment.Student.Name,
                    Age = studentDepartment.Student.Age,
                    Email = studentDepartment.Student.Email,
                    HashPassword = studentDepartment.Student.Password,
                    ConfirmPassword = studentDepartment.Student.ConfirmPassword

                };
                var studentHasher = new PasswordHasher<Student>();
                var userHasher = new PasswordHasher<User>();

                // Hash the password before saving it
                studentDepartment.Student.Password = studentHasher.HashPassword(studentDepartment.Student, studentDepartment.Student.Password);
                user.HashPassword = userHasher.HashPassword(user, user.HashPassword);

                await userRepo.AddAsync(user);
                await userRepo.SaveChangesAsync();

                studentDepartment.Student.UserId = user.Id;

                await studentRepo.AddAsync(studentDepartment.Student);
                await studentRepo.SaveChangesAsync();

                Role studentRole = await roleRepoExtra.GetByNameAsync("student".ToLower());
                Role usertRole = await roleRepoExtra.GetByNameAsync("user".ToLower());
                await userRoleRepoExtra.AddAsync(user.Id, studentRole.Id);
                await userRoleRepoExtra.AddAsync(user.Id, usertRole.Id);
                await userRoleRepo.SaveChangesAsync();

                return RedirectToAction("index");
            }

            var depts = await departmentRepo.GetAllAsync();
            studentDepartment.Departments = depts.ToList();

            return View(studentDepartment);

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            //var student = dbContext.Students.SingleOrDefault(s => s.Id == id);
            var student = await studentRepo.GetByIdAsync(id.Value);
            if (student == null)
            {
                //studentRepo.Dispose();
                return NotFound();
            }
            var depts = await departmentRepo.GetAllAsync();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = student,
                Departments = depts.ToList()
            };
            //departmentRepo.Dispose();
            return View(studentDepartment);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Student student)
        {
            var existingStudent = await studentRepo.GetByIdAsync(student.Id);
            if (existingStudent == null)
                return NotFound();
            var studentWithSameEmail = await studentRepoExtra.GetStudentByEmailAsync(student.Email);
            var userWithSameEmail = await userRepoExtra.GetUserByEmailAsync(student.Email);

            if (studentWithSameEmail != null && studentWithSameEmail.Id != student.Id)
                ModelState.AddModelError("Student.Email", "Email already exists!");
            else if (userWithSameEmail != null && userWithSameEmail.Id != existingStudent.UserId)
                ModelState.AddModelError("Student.Email", "Email already exists!");

            if (ModelState.IsValid)
            {
                existingStudent.Name = student.Name;
                existingStudent.Email = student.Email;
                existingStudent.Age = student.Age;
                existingStudent.DeptNo = student.DeptNo;

                var hasher = new PasswordHasher<Student>();
                existingStudent.Password = hasher.HashPassword(existingStudent, student.Password);

                studentRepo.Update(existingStudent);

                // ===== Update Related User =====
                var relatedUser = await userRepoExtra.GetByIdAsync(existingStudent.UserId);
                if (relatedUser != null)
                {
                    relatedUser.UserName = student.Name;
                    relatedUser.Email = student.Email;
                    relatedUser.Age = student.Age;

                    var userHasher = new PasswordHasher<User>();
                    relatedUser.HashPassword = userHasher.HashPassword(relatedUser, student.Password);

                    userRepo.Update(relatedUser);
                }
                await studentRepo.SaveChangesAsync();
                await userRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            var depts = await departmentRepo.GetAllAsync();
            StudentDepartment model = new StudentDepartment()
            {
                Student = student,
                Departments = depts.ToList()

            };
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = await studentRepo.GetByIdAsync(id.Value);
            if (student == null)
            {
                return NotFound();
            }
            //====== Repository Pattern ======
            studentRepo.Delete(student);
            await studentRepo.SaveChangesAsync();
            //Remove From Student Role
            var existingUser = await userRepoExtra.GetUserByEmailAsync(student.Email);
            Role studentRole = await roleRepoExtra.GetByNameAsync("Student".ToLower());
            UserRole userRole = new UserRole()
            {
                UserId = existingUser.Id,
                RoleId = studentRole.Id
            };
            userRoleRepoExtra.Delete(userRole);
            await userRoleRepo.SaveChangesAsync();
            return RedirectToAction("index");
        }
        [AllowAnonymous]
        public async Task<IActionResult> EmailExist([FromQuery(Name = "Student.Email")] string email, [FromQuery(Name = "Student.Id")] int id)
        {
            bool emailExist = await studentRepoExtra.IsEmailExistAsync(email, id);
            return Json(!emailExist);
        }
    }
}

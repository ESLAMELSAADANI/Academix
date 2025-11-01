using Demo.DAL;
using ModelsLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Demo.Repos;
using Demo.ViewModels;
using ModelsLayer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers
{
    //[Route("hamada/{action=index}/{id:int:max(500)?}")]
    //[Route("hamada/{action=index}/{id:int:max(500)?}")]
    [Authorize]
    public class DepartmentController : Controller
    {
        //ITIDbContext dbContext = new ITIDbContext();
        //===== Repository Design Pattern =======
        //IEntityRepo<Department> departmentRepo = new DepartmentRepo();
        //IIdExist departmentExist = new DepartmentRepo();

        //===== Dependency Injection ======
        EntityRepo<Department> departmentRepo;
        EntityRepo<Course> courseRepo;
        EntityRepo<StudentCourse> studentCourseRepo;
        IStudentCourseRepoExtra StudentCourseRepoExtra;
        IDepartmentRepoExtra departmentRepoExtra;
        public DepartmentController(EntityRepo<Department> _departmentRepo, IDepartmentRepoExtra _departmentRepoExtra, EntityRepo<Course> _courseRepo, EntityRepo<StudentCourse> _studentCourseRepo, IStudentCourseRepoExtra _StudentCourseRepoExtra)
        {
            departmentRepo = _departmentRepo;
            courseRepo = _courseRepo;
            departmentRepoExtra = _departmentRepoExtra;
            studentCourseRepo = _studentCourseRepo;
            StudentCourseRepoExtra = _StudentCourseRepoExtra;
        }
        //[Route("hamada")]
        public async Task<IActionResult> Index(/*[FromServices]IEntityRepo<Department> _departmentRepo*/)
        {
            //var model = dbContext.Department.ToList();
            var model = await departmentRepo.GetAllAsync();
            //departmentRepo.Dispose();
            return View(model);
        }
        //Show The Form Of Add New Department
        [HttpGet] //ActionSelector say that this action work with Get Request Only
        public IActionResult Create()
        {
            return View();
        }
        //Receive Data from request and save data to database.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Department dept)
        {
            if (ModelState.IsValid)
            {
                //dbContext.Department.Add(dept);
                //dbContext.SaveChanges();
                //return RedirectToAction("index");

                await departmentRepo.AddAsync(dept);
                await departmentRepo.SaveChangesAsync();
                //departmentRepo.Dispose();
                return RedirectToAction("index");
            }
            return View();
        }
        // department/details/500   =>  500 will binded to the passed parameter id [Route System Behavior]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)//if user not enter value for id => /department/details
            {
                return BadRequest();
            }
            //var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
            var dept = await departmentRepo.GetByIdAsync(id.Value);
            if (dept == null)//if user enter id not match any dept in DB => /department/details/88888
            {
                //departmentRepo.Dispose();
                return NotFound();
            }
            //departmentRepo.Dispose();
            return View(dept);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)//if user not enter value for id => /department/edit
            {
                return BadRequest();
            }
            //var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
            var dept = await departmentRepo.GetByIdAsync(id.Value);
            if (dept == null)//if user enter id not match any dept in DB => /department/edit/88888
            {
                //departmentRepo.Dispose();
                return NotFound();
            }

            //departmentRepo.Dispose();
            return View(dept);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Department dept)
        {
            if (ModelState.IsValid)
            {
                //dbContext.Department.Update(dept);
                //dbContext.SaveChanges();
                //return RedirectToAction("index");

                departmentRepo.Update(dept);
                await departmentRepo.SaveChangesAsync();
                //departmentRepo.Dispose();
                return RedirectToAction("index");
            }
            var ddept = await departmentRepo.GetByIdAsync(dept.DeptId);
            return View(ddept);
        }
        //For View Department Courses Details and can delete course
        public async Task<IActionResult> DepartmentCourses(int? id)
        {
            var department = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(id.Value);
            return View(department);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartmentCourse(DepartmentCoursesVM dept, int courseID)
        {
            var department = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(dept.Department.DeptId);
            var course = await courseRepo.GetByIdAsync(courseID);
            department.Courses.Add(course);
            await departmentRepo.SaveChangesAsync();
            return RedirectToAction("DepartmentCourses", new { id = dept.Department.DeptId });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartmentCourse(Department dept, int courseID)
        {
            var department = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(dept.DeptId);
            var course = department.Courses.SingleOrDefault(c => c.Id == courseID);
            department.Courses.Remove(course);
            await departmentRepo.SaveChangesAsync();
            return RedirectToAction("DepartmentCourses", new { id = dept.DeptId });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DepartmentAddCourses(int id)
        {
            var department = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(id);
            var coursesInDepartment = department.Courses.ToList();
            var allCourses = await courseRepo.GetAllAsync();
            var otherCourses = allCourses.Except(coursesInDepartment).ToList();

            DepartmentCoursesVM model = new DepartmentCoursesVM()
            {
                Department = department,
                DepartmentCourses = coursesInDepartment,
                OtherCourses = otherCourses
            };

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDepartmentCourses(int? id)
        {
            var department = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(id.Value);
            var departmentCourses = department.Courses.ToList();
            var allCourses = await courseRepo.GetAllAsync();
            var otherCourses = allCourses.Except(departmentCourses).ToList();

            DepartmentCoursesVM model = new DepartmentCoursesVM()
            {
                Department = department,
                DepartmentCourses = departmentCourses,
                OtherCourses = otherCourses
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDepartmentCourses(DepartmentCoursesVM model)
        {
            var dept = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(model.Department.DeptId);
            if (dept == null)
                return NotFound();
            if (model.CoursesToDelete.Count == 0 && model.CoursesToAdd.Count == 0)
                return RedirectToAction("editDepartmentCourses", new { id = dept.DeptId });
            if (model.CoursesToDelete != null)
            {
                foreach (var id in model.CoursesToDelete)
                {
                    var course = dept.Courses.SingleOrDefault(c => c.Id == id);
                    dept.Courses.Remove(course);
                }
            }
            if (model.CoursesToAdd != null)
            {
                foreach (var id in model.CoursesToAdd)
                {
                    var course = await courseRepo.GetByIdAsync(id);
                    dept.Courses.Add(course);
                }
            }
            await departmentRepo.SaveChangesAsync();
            return RedirectToAction("DepartmentCourses", new { id = dept.DeptId });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var department = await departmentRepo.GetByIdAsync(id.Value);
            if (department == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                //var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
                //dbContext.Department.Remove(dept);
                //dbContext.SaveChanges();
                //return RedirectToAction("index");


                departmentRepo.Delete(department);
                await departmentRepo.SaveChangesAsync();
                //departmentRepo.Dispose();
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Department dept)
        {
            if (ModelState.IsValid)
            {
                ////var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
                //dbContext.Department.Remove(dept);
                //dbContext.SaveChanges();
                //return RedirectToAction("index");

                departmentRepo.Delete(dept);
                await departmentRepo.SaveChangesAsync();
                //departmentRepo.Dispose();
                return RedirectToAction("index");
            }
            return View("edit", dept);
        }
        [AllowAnonymous]
        public IActionResult IdExist(int DeptId)
        {
            bool exist = departmentRepoExtra.IsIdExist(DeptId);
            return Json(!exist);
        }
        public async Task<IActionResult> ViewCourseStudents(int crsId, int deptID)
        {
            var department = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(deptID);
            var course = await courseRepo.GetByIdAsync(crsId);

            var departmentCourseStudents = department.Students.Where(s => s.StudentCourses.Any(sc => sc.CourseId == crsId)).ToList();
            var otherStudents = department.Students.Where(s => !s.StudentCourses.Any(sc => sc.StudentId == s.Id)).ToList();


            var studentDegrees = departmentCourseStudents.Select(s => new StudentDegreeVM
            {
                StudentId = s.Id,
                CourseId = crsId,
                Degree = s.StudentCourses.Single(sc => sc.CourseId == crsId).Degree
            }).ToList();
            var otherStudentsDegrees = otherStudents.Select(s => new StudentDegreeVM
            {
                StudentId = s.Id,
                CourseId = crsId,
                Degree = null
            }).ToList();
            DepartmentCourseStudentsVM model = new DepartmentCourseStudentsVM()
            {
                Course = course,
                Department = department,
                StudentCourseDegrees = studentDegrees,
                OtherStudents = otherStudentsDegrees

            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDegree(DepartmentCourseStudentsVM model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model.StudentCourseDegrees)
                {
                    var studentCourse = await StudentCourseRepoExtra.GetByStdIdCrsIdAsync(item.StudentId, item.CourseId);
                    if (studentCourse != null)
                        studentCourse.Degree = item.Degree;
                }
                await studentCourseRepo.SaveChangesAsync();
                return RedirectToAction("ViewCourseStudents", new { crsId = model.Course.Id, deptID = model.Department.DeptId });
            }
            var department = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(model.Department.DeptId);
            var course = await courseRepo.GetByIdAsync(model.Course.Id);

            var departmentCourseStudents = department.Students.Where(s => s.StudentCourses.Any(sc => sc.CourseId == model.Course.Id)).ToList();
            var otherStudents = department.Students.Where(s => !s.StudentCourses.Any(sc => sc.StudentId == s.Id)).ToList();


            var studentDegrees = departmentCourseStudents.Select(s => new StudentDegreeVM
            {
                StudentId = s.Id,
                CourseId = model.Course.Id,
                Degree = s.StudentCourses.Single(sc => sc.CourseId == model.Course.Id).Degree
            }).ToList();
            var otherStudentsDegrees = otherStudents.Select(s => new StudentDegreeVM
            {
                StudentId = s.Id,
                CourseId = model.Course.Id,
                Degree = null
            }).ToList();
            DepartmentCourseStudentsVM newModel = new DepartmentCourseStudentsVM()
            {
                Course = course,
                Department = department,
                StudentCourseDegrees = studentDegrees,
                OtherStudents = otherStudentsDegrees

            };
            return View("ViewCourseStudents", newModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnrollSelectedStudents(DepartmentCourseStudentsVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedStudentIds != null && model.SelectedStudentIds.Any())
                {
                    for (int i = 0; i < model.SelectedStudentIds.Count; i++)
                    {
                        // Add a StudentCourse record for each selected student
                        await studentCourseRepo.AddAsync(new StudentCourse
                        {
                            StudentId = model.SelectedStudentIds[i],
                            CourseId = model.Course.Id,
                            Degree = model.OtherStudents[i].Degree
                        });
                    }
                    await studentCourseRepo.SaveChangesAsync();
                }

                return RedirectToAction("ViewCourseStudents", new { crsId = model.Course.Id, deptID = model.Department.DeptId });
            }
            var department = await departmentRepoExtra.GetDepartmentByIdWithNavigationPropsAsync(model.Department.DeptId);
            var course = await courseRepo.GetByIdAsync(model.Course.Id);

            var departmentCourseStudents = department.Students.Where(s => s.StudentCourses.Any(sc => sc.CourseId == model.Course.Id)).ToList();
            var otherStudents = department.Students.Where(s => !s.StudentCourses.Any(sc => sc.StudentId == s.Id)).ToList();


            var studentDegrees = departmentCourseStudents.Select(s => new StudentDegreeVM
            {
                StudentId = s.Id,
                CourseId = model.Course.Id,
                Degree = s.StudentCourses.Single(sc => sc.CourseId == model.Course.Id).Degree
            }).ToList();
            var otherStudentsDegrees = otherStudents.Select(s => new StudentDegreeVM
            {
                StudentId = s.Id,
                CourseId = model.Course.Id,
                Degree = null
            }).ToList();
            DepartmentCourseStudentsVM newModel = new DepartmentCourseStudentsVM()
            {
                Course = course,
                Department = department,
                StudentCourseDegrees = studentDegrees,
                OtherStudents = otherStudentsDegrees

            };
            return View("ViewCourseStudents", newModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudentCourse(int stdID, int crsID, int deptId)
        {
            var studentCourse = await StudentCourseRepoExtra.GetByStdIdCrsIdAsync(stdID, crsID);
            studentCourseRepo.Delete(studentCourse);
            await studentCourseRepo.SaveChangesAsync();
            return RedirectToAction("ViewCourseStudents", new { crsId = crsID, deptID = deptId });
        }
    }
}

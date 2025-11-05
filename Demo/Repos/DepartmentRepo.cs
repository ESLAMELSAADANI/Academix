using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;
using System.Threading.Tasks;

namespace Demo.Repos
{
    public interface IDepartmentRepoExtra : IEntityRepo<Department>
    {
        bool IsIdExist(int id);
        Task<Department> DetailsAsync(int id);
        Task<Department> GetDepartmentByIdWithNavigationPropsAsync(int id);
        Task<Department> GetFirstDeptAsync();
        Task SoftDeleteAsync(int id);
    }

    public class DepartmentRepo : EntityRepo<Department>, IDepartmentRepoExtra
    {
        public DepartmentRepo(ITIDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task<Department> DetailsAsync(int id)
        {
            return await dbContext.Department.SingleOrDefaultAsync(d => d.DeptId == id);
        }
        public async Task<Department> GetDepartmentByIdWithNavigationPropsAsync(int id)
        {
            return await dbContext.Department.Include(d => d.Courses).Include(d => d.Students).ThenInclude(d => d.StudentCourses).SingleOrDefaultAsync(d => d.DeptId == id);
        }
        public async Task<Department> GetFirstDeptAsync()
        {
            return await dbContext.Department.FirstOrDefaultAsync();
        }
        public bool IsIdExist(int id)
        {
            return dbContext.Department.Any(d => d.DeptId == id);
        }
        public async Task SoftDeleteAsync(int id)
        {
            var dept = dbContext.Department.Include(d => d.Students).SingleOrDefault(d => d.DeptId == id);
            if (dept.Students.Count == 0)
                dbContext.Department.Remove(dept);//Hard Delete From DB
            //Soft Delete
            else
            {
                dept.IsActive = false;//Soft Delete, Just Marked In DB As InActive
                Update(dept);
                var studentsInDept = dbContext.Students.Where(s => s.DeptNo == dept.DeptId);
                await SaveChangesAsync();
                var deptsActive = dbContext.Department.Where(d => d.IsActive != false);
                if (deptsActive.Count() == 0)
                    dbContext.Students.ExecuteDelete();
                else
                {
                    var randomDept = deptsActive.First();
                    foreach (var std in studentsInDept)
                    {
                        std.DeptNo = randomDept.DeptId;
                        dbContext.Students.Update(std);
                    }
                }

            }

        }
    }
}

using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;
using ModelsLayer.Models;
using System.Reflection.Metadata.Ecma335;

namespace Demo.Repos
{
    public interface IStudentRepoExtra : IEntityRepo<Student>
    {
        Task<bool> IsEmailExistAsync(string email, int id);
        Task<Student> DetailsAsync(int id);
        Task<Student> GetStudentByEmailAsync(string email);
        Task<List<Student>> GetAllWithDepartmentsAsync();
        Task<Student> GetStudentByUserIdAsync(int userId);
    }
    public class StudentRepo : EntityRepo<Student>, IStudentRepoExtra
    {
        IUserRepoExtra userRepoExtra;
        public StudentRepo(ITIDbContext _dbContext, IUserRepoExtra _userRepoExtra) : base(_dbContext)
        {
            userRepoExtra = _userRepoExtra;
        }

        //ITIDbContext dbContext = new ITIDbContext();

        ////====== Dependency Injection =======
        //ITIDbContext dbContext;
        //public StudentRepo(ITIDbContext _dbContext)////Constructor Injection [DIC Will Inject The Object Here]
        //{
        //    dbContext = _dbContext;
        //}


        public async Task<Student> DetailsAsync(int id)
        {
            return await dbContext.Students.Include(s => s.Department).SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Student>> GetAllWithDepartmentsAsync()
        {
            return await dbContext.Students.Include(s => s.Department).ToListAsync();
        }

        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await dbContext.Students.AsNoTracking().SingleOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Student> GetStudentByUserIdAsync(int userId)
        {
            return await dbContext.Students.AsNoTracking().SingleOrDefaultAsync(s => s.UserId == userId);
        }

        //Not need to use it, bcz dependency injection automatically dispose object created after lifetime of it end
        //public void Dispose()
        //{
        //    dbContext.Dispose();
        //}


        public async Task<bool> IsEmailExistAsync(string email, int id)
        {
            // When adding a new Student (Id == 0)
            if (id == 0)
            {
                bool emailExistInUsers = dbContext.Users.Any(s => s.Email == email);

                if (!emailExistInUsers)
                    return false;
                return true;
            }

            // When editing an existing Student
            var existingStudent = await GetStudentByEmailAsync(email);
            var existingUser = await userRepoExtra.GetUserByEmailAsync(email);
            if (existingStudent == null)
            {
                if (existingUser != null)
                    return true;
                return false;
            }
            // Email exists but belongs to the same Student being edited → valid
            if (existingStudent.Id == id)
                return false;

            // Email exists and belongs to another Student → invalid
            return true;
        }
    }
}

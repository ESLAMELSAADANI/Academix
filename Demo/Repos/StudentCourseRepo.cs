using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;

namespace Demo.Repos
{
    public interface IStudentCourseRepoExtra : IEntityRepo<StudentCourse>
    {
        public Task<StudentCourse> GetByStdIdCrsIdAsync(int stdId, int crsId);
    }
    public class StudentCourseRepo : EntityRepo<StudentCourse>, IStudentCourseRepoExtra
    {
        public StudentCourseRepo(ITIDbContext _dbContext) : base(_dbContext)
        {
        }
        public async Task<StudentCourse> GetByStdIdCrsIdAsync(int stdId, int crsId)
        {
            return await dbContext.StudentCourses.SingleOrDefaultAsync(sc => sc.StudentId == stdId && sc.CourseId == crsId);
        }
    }
}

using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;

namespace Demo.Repos
{
    public interface ICourseRepoExtra : IEntityRepo<Course>
    {
        public Task<Course> DetailsAsync(int id);
    }
    public class CourseRepo : EntityRepo<Course>, ICourseRepoExtra
    {

        public CourseRepo(ITIDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task<Course> DetailsAsync(int id)
        {
            return await dbContext.Courses.SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}

using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;

namespace Demo.Repos
{
    public interface IUserRoleRepoExtra : IEntityRepo<UserRole>
    {
        public Task AddAsync(int userId, int roleId);
        public Task<UserRole> GetAsync(int userId, int roleId);
    }
    public class UserRoleRepo : EntityRepo<UserRole>, IUserRoleRepoExtra
    {
        public UserRoleRepo(ITIDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task AddAsync(int userId, int roleId)
        {
            var model = new UserRole()
            {
                UserId = userId,
                RoleId = roleId
            };
            await dbContext.UserRoles.AddAsync(model);
        }

        public Task<UserRole> GetAsync(int userId, int roleId)
        {
            return dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.RoleId == roleId && ur.UserId == userId);
        }
    }
}

using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;

namespace Demo.Repos
{
    public interface IRoleRepoExtra : IEntityRepo<Role>
    {
        public bool IsRoleExist(string roleName);
        public Task<Role> GetByNameAsync(string roleName);
        public Task<List<Role>> GetUserRolesAsync(int userID);
        public Task<List<Role>> GetUserAnotherRolesAsync(int userID);

    }
    public class RoleRepo : EntityRepo<Role>, IRoleRepoExtra
    {
        public RoleRepo(ITIDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            return await dbContext.Roles.FirstOrDefaultAsync(ur => ur.RoleName.ToLower() == roleName);
        }

        public Task<List<Role>> GetUserAnotherRolesAsync(int userID)
        {
            return dbContext.Roles.Include(r => r.UserRoles).Where(r => !r.UserRoles.Any(ur => ur.UserId == userID)).ToListAsync();
        }

        public Task<List<Role>> GetUserRolesAsync(int userID)
        {
            return dbContext.Roles.Include(r => r.UserRoles).Where(r => r.UserRoles.Any(ur => ur.UserId == userID)).ToListAsync();
        }

        public bool IsRoleExist(string roleName)
        {
            return dbContext.Roles.Any(r => r.RoleName == roleName);
        }
    }
}

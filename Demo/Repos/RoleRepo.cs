using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;

namespace Demo.Repos
{
    public interface IRoleRepoExtra : IEntityRepo<Role>
    {
        public Task<bool> IsRoleExistAsync(string roleName,int id);
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
        public async Task<bool> IsRoleExistAsync(string roleName,int id)
        {
            // When adding a new Role (Id == 0)
            if (id == 0)
            {
                bool roleExists = dbContext.Roles.Any(r => r.RoleName.ToLower() == roleName.ToLower());
                return roleExists; // true if role exist
            }

            // When editing an existing role
            var existingRole = await GetByNameAsync(roleName);

            if (existingRole == null)
                return false; // Role doesn't exist in DB → valid

            // Role exists but belongs to the same user being edited → valid
            if (existingRole.Id == id)
                return false;

            // Role exists and belongs to another user → invalid
            return true;
        }
    }
}

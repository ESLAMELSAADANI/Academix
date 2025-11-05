using Demo.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;
using System.Threading.Tasks;

namespace Demo.Repos
{
    public interface IUserRepoExtra : IEntityRepo<User>
    {
        public Task<bool> emailExistAsync(string email, int id);
        public Task<User> GetUserByIdWithRolesAsync(int id);
        public Task<User> GetUserByEmailPasswordAsync(string email, string password);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> GetUserByUserNameAsync(string username);
    }
    public class UserRepo : EntityRepo<User>, IUserRepoExtra
    {
        public UserRepo(ITIDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task<bool> emailExistAsync(string email,int id)
        {
            // When adding a new user (Id == 0)
            if (id == 0)
            {
                bool emailExist = dbContext.Users.Any(u => u.Email == email);
                if (!emailExist)
                    return false;
                return true;
            }

            // When editing an existing user
            var existingUser = await GetUserByEmailAsync(email);

            if (existingUser == null)
                return false; // Email doesn't exist in DB → valid

            // Email exists but belongs to the same user being edited → valid
            if (existingUser.Id == id)
                return false;

            // Email exists and belongs to another user → invalid
            return true;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> GetUserByEmailPasswordAsync(string email, string password)
        {
            var user = await dbContext.Users.Include(u => u.UsersRole).FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.HashPassword, password);

            if (result == PasswordVerificationResult.Success)
                return user;
            return null;
        }
        public async Task<User> GetUserByIdWithRolesAsync(int id)
        {
            return await dbContext.Users.Include(u => u.UsersRole).FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
        }
    }
}

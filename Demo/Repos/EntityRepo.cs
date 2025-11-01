using Demo.DAL;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repos
{
    public class EntityRepo<T> : IEntityRepo<T> where T : class
    {
        protected readonly ITIDbContext dbContext;

        public EntityRepo(ITIDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }
    }
}

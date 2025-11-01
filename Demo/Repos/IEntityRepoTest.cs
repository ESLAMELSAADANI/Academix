namespace Demo.Repos
{
    public interface IEntityRepoTest<T> where T : class
    {
        public List<T> GetAll();
        public T GetById(int id);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        int SaveChanges();
    }
}

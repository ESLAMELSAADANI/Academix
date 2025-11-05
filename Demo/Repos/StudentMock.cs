using ModelsLayer.Models;

namespace Demo.Repos
{
    public class StudentMock : IEntityRepoTest<Student>
    {
        List<Student> students = new List<Student>()
        {
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
        };

        public void Add(Student entity)
        {
            students.Add(entity);
        }
        public void Delete(Student entity)
        {
            students.Remove(entity);
        }
        public List<Student> GetAll()
        {
            return students;
        }
        public Student GetById(int id)
        {
            return students.SingleOrDefault(s => s.Id == id);
        }
        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
        public void Update(Student entity)
        {
            var student = students.SingleOrDefault(s => s.Id == entity.Id);
            student.Id = entity.Id;
            student.Name = entity.Name;
            student.Age = entity.Age;
            student.Password = entity.Password;
            student.ConfirmPassword = entity.ConfirmPassword;
            student.DeptNo = entity.DeptNo;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ModelsLayer;
using ModelsLayer.Models;
using System.Data;

namespace Demo.DAL
{
    public class ITIDbContext : DbContext
    {

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        //protected ITIDbContext()
        //{
        //}

        public ITIDbContext(DbContextOptions options) : base(options)
        {

        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ITIMVC;Integrated Security=True;Trust Server Certificate=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(c =>
            {
                c.HasData(
                    new Course() { Id = 1, CrsName = "OS", CrsDuration = 120 },
                    new Course() { Id = 2, CrsName = "Network", CrsDuration = 100 },
                    new Course() { Id = 3, CrsName = "OOP", CrsDuration = 200 },
                    new Course() { Id = 4, CrsName = "LINQ", CrsDuration = 150 },
                    new Course() { Id = 5, CrsName = "DS", CrsDuration = 170 }
                    );
            });
            modelBuilder.Entity<Department>(d =>
            {
                d.HasData(
                    new Department() { DeptId = 100, DeptName = "CS", Capacity = 50 },
                    new Department() { DeptId = 200, DeptName = "Cyber", Capacity = 25 },
                    new Department() { DeptId = 300, DeptName = "Java", Capacity = 30 },
                    new Department() { DeptId = 400, DeptName = "Cross", Capacity = 45 }
                    );
            });
            modelBuilder.Entity<Role>(r =>
            {
                r.HasData(
                    new Role() {Id=2, RoleName = "User" },
                    new Role() {Id=3, RoleName = "Student" },
                    new Role() {Id = 1, RoleName = "Admin" }
                    );
            });
            modelBuilder.Entity<UserRole>(ur =>
            {
                ur.HasData(new UserRole() { UserId=1,RoleId=1});
            });
        }
    }
}

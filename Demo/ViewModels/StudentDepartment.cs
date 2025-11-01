using ModelsLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.ViewModels
{
    public class StudentDepartment
    {
        public Student Student { get; set; }
        public List<Department> Departments { get; set; } = new();
    }
}

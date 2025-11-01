using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer
{
    public class Course
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter The Name!")]
        public string CrsName { get; set; }
        [Range(2, 200, ErrorMessage = "Duration Must Be Between 2-200 H"), Required(ErrorMessage = "Enter Duration!")]
        public int CrsDuration { get; set; }

        public List<Department> Departments { get; set; } = new List<Department>();
        public List<StudentCourse> CourseStudents { get; set; } = new List<StudentCourse>();
    }
}

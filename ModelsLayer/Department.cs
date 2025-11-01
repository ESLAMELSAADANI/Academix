using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLayer.Models
{
    [PrimaryKey("DeptId")]
    public class Department
    {
        [Required(ErrorMessage = "Enter Id!")]
        [Remote("IdExist","Department", ErrorMessage = "This Id Exist In DB Enter Another ID!")]
        public int DeptId { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Min Length 2"), Required(ErrorMessage = "Enter Dept Name!")]
        public string DeptName { get; set; }
        [Required(ErrorMessage = "Enter Capacity!")]
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Course> Courses { get; set; } = new List<Course>();

        public override string ToString()
        {
            return $"DeptId= {DeptId} - DeptName= {DeptName} - Capacity= {Capacity}";
        }

    }
}

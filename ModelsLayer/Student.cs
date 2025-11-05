using ModelsLayer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace ModelsLayer.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Your Name!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name Must be less than 50 chars and grater than 3 chars")]//Max Validation mapped to DB, min validation not mapped
        public string Name { get; set; }
        [Range(10, 50, ErrorMessage = "Age must be between 10 and 50 years.")]//just validation in App, Not mapped to DB
        public int Age { get; set; }
        [EmailAddress]
        [Remote("EmailExist", "Student", AdditionalFields = "Id", ErrorMessage = "Email Exists!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required!"), MinLength(6, ErrorMessage = "Password Can't be less than 6")]
        public string Password { get; set; }
        [NotMapped, Compare("Password", ErrorMessage = "Not Match!")]
        public string ConfirmPassword { get; set; }
        [ForeignKey("Department")]
        //[Required(ErrorMessage = "Please select a department!")]
        public int? DeptNo { get; set; }
        public Department? Department { get; set; }
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Age: {Age}";
        }
    }
}

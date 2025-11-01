using Microsoft.AspNetCore.Mvc;
using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer
{
    public class User
    {
        public int Id { get; set; }
        [Required, StringLength(60)]
        public string UserName { get; set; }
        [Required, EmailAddress, RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        [Remote("EmailExist", "user", AdditionalFields = "Id", ErrorMessage = "Email Exists!")]
        public string Email { get; set; }
        [Required, Range(10, 60)]
        public int Age { get; set; }
        [Required(ErrorMessage = "The Password Field Is Required.")]
        public string HashPassword { get; set; }
        [NotMapped, Compare("HashPassword", ErrorMessage = "Don't Match!")]
        public string ConfirmPassword { get; set; }

        public Student? Student { get; set; }

        public ICollection<UserRole> UsersRole { get; set; } = new List<UserRole>();
    }
}

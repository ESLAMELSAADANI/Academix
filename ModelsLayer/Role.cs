using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        [Remote("RoleExist", "Role", AdditionalFields = "Id", ErrorMessage = "This Role Exist In DB!")]
        public string RoleName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}

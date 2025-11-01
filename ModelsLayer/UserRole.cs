using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer
{
    [PrimaryKey("UserId", "RoleId")]
    public class UserRole
    {
        [ForeignKey("User"), Required]
        public int UserId { get; set; }
        [ForeignKey("Role"), Required]
        public int RoleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}

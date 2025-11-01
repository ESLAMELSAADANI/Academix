using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class LoginVM
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

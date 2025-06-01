using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Models.LoginDTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(100)]
        public string AccountName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [Range(1, 2)] // 1 = Admin, 2 = Staff
        public int AccountRole { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Models.LoginDTOs
{
    public class LoginResponseDto
    {
        public short AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int AccountRole { get; set; }
        public string Token { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}

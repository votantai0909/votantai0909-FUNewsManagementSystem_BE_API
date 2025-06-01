using FUNewsManagementSystem.Services.Models.LoginDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest);
        Task<ApiResponseDto<LoginResponseDto>> RegisterAsync(RegisterRequestDto registerRequest);
        Task<bool> ValidateTokenAsync(string token);
        string GenerateJwtToken(short accountId, string email, int role);
    }
}

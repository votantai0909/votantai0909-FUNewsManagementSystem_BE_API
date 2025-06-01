using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Repositories.Interfaces;
using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Models.LoginDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthService(ISystemAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task<ApiResponseDto<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest)
        {
            try
            {
                var account = await _accountRepository.GetByEmailAsync(loginRequest.Email);

                if (account == null)
                {
                    return new ApiResponseDto<LoginResponseDto>
                    {
                        Success = false,
                        Message = "Invalid email or password",
                        Errors = new List<string> { "Account not found" }
                    };
                }

                if (!VerifyPassword(loginRequest.Password, account.AccountPassword!))
                {
                    return new ApiResponseDto<LoginResponseDto>
                    {
                        Success = false,
                        Message = "Invalid email or password",
                        Errors = new List<string> { "Password incorrect" }
                    };
                }

                var token = GenerateJwtToken(account.AccountId, account.AccountEmail!, account.AccountRole ?? 0);

                var response = new LoginResponseDto
                {
                    AccountId = account.AccountId,
                    AccountName = account.AccountName!,
                    Email = account.AccountEmail!,
                    AccountRole = account.AccountRole ?? 0,
                    Token = token,
                    Message = "Login successful"
                };

                return new ApiResponseDto<LoginResponseDto>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "An error occurred during login",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponseDto<LoginResponseDto>> RegisterAsync(RegisterRequestDto registerRequest)
        {
            try
            {
                if (await _accountRepository.EmailExistsAsync(registerRequest.Email))
                {
                    return new ApiResponseDto<LoginResponseDto>
                    {
                        Success = false,
                        Message = "Email already exists",
                        Errors = new List<string> { "An account with this email already exists" }
                    };
                }

                var hashedPassword = HashPassword(registerRequest.Password);

                // Generate new AccountId
                var nextAccountId = await GetNextAccountIdAsync();

                var newAccount = new SystemAccount
                {
                    AccountId = nextAccountId,
                    AccountName = registerRequest.AccountName,
                    AccountEmail = registerRequest.Email,
                    AccountPassword = hashedPassword,
                    AccountRole = registerRequest.AccountRole
                };

                var createdAccount = await _accountRepository.CreateAsync(newAccount);

                /*var token = GenerateJwtToken(createdAccount.AccountId, createdAccount.AccountEmail!, createdAccount.AccountRole ?? 0);*/

                var response = new LoginResponseDto
                {
                    AccountId = createdAccount.AccountId,
                    AccountName = createdAccount.AccountName!,
                    Email = createdAccount.AccountEmail!,
                    AccountRole = createdAccount.AccountRole ?? 0,
                    /*Token = token,*/
                    Message = "Registration successful"
                };

                return new ApiResponseDto<LoginResponseDto>
                {
                    Success = true,
                    Message = "Registration successful",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "An error occurred during registration",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public string GenerateJwtToken(short accountId, string email, int role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

        private async Task<short> GetNextAccountIdAsync()
        {
            var allAccounts = await _accountRepository.GetAllAsync();

            if (!allAccounts.Any())
            {
                return 1; 
            }

            var maxId = allAccounts.Max(a => a.AccountId);
            return (short)(maxId + 1);
        }
    }
}

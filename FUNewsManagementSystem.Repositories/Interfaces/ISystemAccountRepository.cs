using FUNewsManagementSystem.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repositories.Interfaces
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<SystemAccount?> GetByIdAsync(short accountId);
        Task<bool> EmailExistsAsync(string email);
        Task<SystemAccount> CreateAsync(SystemAccount account);
        Task<SystemAccount> UpdateAsync(SystemAccount account);
        Task<bool> DeleteAsync(short accountId);
        Task<IEnumerable<SystemAccount>> GetAllAsync();
    }
}

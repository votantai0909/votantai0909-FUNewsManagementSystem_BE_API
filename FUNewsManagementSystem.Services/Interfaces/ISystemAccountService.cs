using FUNewsManagementSystem.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Interfaces
{
    public interface ISystemAccountService
    {
        Task<bool> CreateSystemAccount(SystemAccount systemAccount);
        Task<bool> DeleteSystemAccount(short id);
        Task<SystemAccount> GetSystemAccount(short id);
        Task<SystemAccount> GetSystemAccount(string username);
        Task<IEnumerable<SystemAccount>> GetSystemAccounts(int index, int pageSize);
        Task<bool> UpdateSystemAccount(SystemAccount systemAccount);
        Task<SystemAccount> GetSystemAccountByEmail(string email);
        Task<SystemAccount?> GetSystemAccountById(short id);
    }
}

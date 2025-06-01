using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repositories.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FUNewsManagementDbContext _context;

        public SystemAccountRepository(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(x => x.AccountEmail == email);
        }

        public async Task<SystemAccount?> GetByIdAsync(short accountId)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.SystemAccounts
                .AnyAsync(x => x.AccountEmail == email);
        }

        public async Task<SystemAccount> CreateAsync(SystemAccount account)
        {
            _context.SystemAccounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<SystemAccount> UpdateAsync(SystemAccount account)
        {
            _context.SystemAccounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> DeleteAsync(short accountId)
        {
            var account = await GetByIdAsync(accountId);
            if (account == null) return false;

            _context.SystemAccounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }
    }
}

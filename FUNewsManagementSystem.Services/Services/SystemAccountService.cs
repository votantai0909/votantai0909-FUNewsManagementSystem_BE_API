using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Repositories.Interfaces;
using FUNewsManagementSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateSystemAccount(SystemAccount systemAccount)
        {
            var repository = _unitOfWork.GetRepository<SystemAccount>();

            // Kiểm tra trùng email
            var existing = await repository.FindAsync(sa => sa.AccountEmail == systemAccount.AccountEmail);
            if (existing != null)
                return false;

            // Lấy ID lớn nhất hiện tại và +1
            var allAccounts = await repository.GetAllAsync(); 
            short nextId = (short)((allAccounts.Any() ? allAccounts.Max(x => x.AccountId) : 0) + 1);

            systemAccount.AccountId = nextId;

            await repository.InsertAsync(systemAccount);
            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<bool> UpdateSystemAccount(SystemAccount systemAccount)
        {
            var repository = _unitOfWork.GetRepository<SystemAccount>();
            var existing = await repository.GetByIdAsync(systemAccount.AccountId);

            if (existing == null)
                return false;

            // Cập nhật các trường
            existing.AccountName = systemAccount.AccountName;
            existing.AccountPassword = systemAccount.AccountPassword;
            existing.AccountEmail = systemAccount.AccountEmail;
            existing.AccountRole = systemAccount.AccountRole;

            await repository.UpdateAsync(existing);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteSystemAccount(short id)
        {
            var repository = _unitOfWork.GetRepository<SystemAccount>();
            var existing = await repository.GetByIdAsync(id);

            if (existing == null)
                return false;

            await repository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<SystemAccount?> GetSystemAccount(short id)
        {
            var repository = _unitOfWork.GetRepository<SystemAccount>();
            return await repository.GetByIdAsync(id);
        }

        public async Task<SystemAccount?> GetSystemAccount(string username)
        {
            var repository = _unitOfWork.GetRepository<SystemAccount>();
            return await repository.FindAsync(sa => sa.AccountName == username);
        }

        public async Task<SystemAccount?> GetSystemAccountByEmail(string email)
        {
            var repository = _unitOfWork.GetRepository<SystemAccount>();
            return await repository.FindAsync(sa => sa.AccountEmail == email);
        }

        public async Task<SystemAccount?> GetSystemAccountById(short id)
        {
            var repository = _unitOfWork.GetRepository<SystemAccount>();
            return await repository.FindAsync(sa => sa.AccountId == id);
        }

        public async Task<IEnumerable<SystemAccount>> GetSystemAccounts(int page, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<SystemAccount>();
            return await repository.GetAsync(page, pageSize);
        }
    }
}

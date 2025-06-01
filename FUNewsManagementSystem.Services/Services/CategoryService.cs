using AutoMapper;
using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Repositories.Interfaces;
using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Models.CategoriesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int index, int pageSize)
        {
            var repo = _unitOfWork.GetRepository<Category>();
            var categories = await repo.GetAsync(index, pageSize);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(short id)
        {
            var repo = _unitOfWork.GetRepository<Category>();
            var category = await repo.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var repo = _unitOfWork.GetRepository<Category>();
            var category = _mapper.Map<Category>(dto);
            await repo.InsertAsync(category);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var repo = _unitOfWork.GetRepository<Category>();
            var category = await repo.GetByIdAsync(dto.CategoryId);
            if (category == null) return false;

            category.CategoryName = dto.CategoryName;
            await repo.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(short id)
        {
            var repo = _unitOfWork.GetRepository<Category>();
            var category = await repo.GetByIdAsync(id);
            if (category == null || (category.NewsArticles != null && category.NewsArticles.Any()))
                return false;

            await repo.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}

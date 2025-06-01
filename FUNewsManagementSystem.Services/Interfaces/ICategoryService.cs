using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Services.Models.CategoriesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int index, int pageSize);
        Task<CategoryDto?> GetCategoryByIdAsync(short id);
        Task<bool> CreateCategoryAsync(CreateCategoryDto dto);
        Task<bool> UpdateCategoryAsync(UpdateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(short id);
    }
}

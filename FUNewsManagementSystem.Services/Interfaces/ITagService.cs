using FUNewsManagementSystem.Services.Models.TagDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllPagedAsync(int page, int pageSize);
        Task<TagDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateTagDto dto);
        Task<bool> UpdateAsync(UpdateTagDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

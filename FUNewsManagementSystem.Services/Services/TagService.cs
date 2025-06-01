using AutoMapper;
using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Repositories.Interfaces;
using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Models.TagDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllPagedAsync(int page, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<Tag>();
            var tags = await repository.GetAsync(page, pageSize);
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<TagDto?> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Tag>();
            var tag = await repository.GetByIdAsync(id);
            return tag != null ? _mapper.Map<TagDto>(tag) : null;
        }

        public async Task<bool> CreateAsync(CreateTagDto dto)
        {
            var repository = _unitOfWork.GetRepository<Tag>();

            // Lấy ID lớn nhất hiện tại
            var maxIdEntity = (await repository.GetAllAsync())
                              .OrderByDescending(t => t.TagId)
                              .FirstOrDefault();

            int newId = (maxIdEntity != null) ? maxIdEntity.TagId + 1 : 1;

            var entity = _mapper.Map<Tag>(dto);
            entity.TagId = newId; // gán ID mới

            await repository.InsertAsync(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<bool> UpdateAsync(UpdateTagDto dto)
        {
            var repository = _unitOfWork.GetRepository<Tag>();
            var existing = await repository.GetByIdAsync(dto.TagId);
            if (existing == null) return false;

            existing.TagName = dto.TagName;
            existing.Note = dto.Note;

            await repository.UpdateAsync(existing);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Tag>();
            var existing = await repository.GetByIdAsync(id);
            if (existing == null) return false;

            await repository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}

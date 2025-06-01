using AutoMapper;
using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Models.AccoutDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "0")]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountService _service;
        private readonly IMapper _mapper;

        public SystemAccountController(ISystemAccountService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSystemAccountDto dto)
        {
            var account = _mapper.Map<SystemAccount>(dto);
            var result = await _service.CreateSystemAccount(account);

            if (!result)
                return BadRequest("Email already exists.");

            return Ok("System account created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateSystemAccountDto dto)
        {
            var account = _mapper.Map<SystemAccount>(dto);
            var result = await _service.UpdateSystemAccount(account);

            if (!result)
                return NotFound("System account not found.");

            return Ok("System account updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var result = await _service.DeleteSystemAccount(id);

            if (!result)
                return NotFound("System account not found.");

            return Ok("System account deleted successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var account = await _service.GetSystemAccount(id);

            if (account == null)
                return NotFound("System account not found.");

            return Ok(_mapper.Map<SystemAccountDto>(account));
        }

        [HttpGet("by-username/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var account = await _service.GetSystemAccount(username);

            if (account == null)
                return NotFound("System account not found.");

            return Ok(_mapper.Map<SystemAccountDto>(account));
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var account = await _service.GetSystemAccountByEmail(email);

            if (account == null)
                return NotFound("System account not found.");

            return Ok(_mapper.Map<SystemAccountDto>(account));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var accounts = await _service.GetSystemAccounts(page, pageSize);
            var result = _mapper.Map<IEnumerable<SystemAccountDto>>(accounts);
            return Ok(result);
        }
    }
}

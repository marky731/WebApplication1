using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ApiResponse<List<RoleDto>>> GetRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _roleService.GetAllRoles(pageNumber, pageSize);
        }

        [HttpGet("{roleId}")]
        public async Task<ApiResponse<RoleDto>> GetSingleRole(int roleId)
        {
            return await _roleService.GetRoleById(roleId);
        }

        [HttpPut]
        public async Task<ApiResponse<RoleDto>> EditRole(RoleDto roleDto)
        {
            return await _roleService.UpdateRole(roleDto);
        }

        [HttpPost]
        public async Task<ApiResponse<RoleDto>> AddRole(RoleDto roleDto)
        {
            return await _roleService.CreateRole(roleDto);
        }

        [HttpDelete]
        public async Task<ApiResponse<string?>> DeleteRole(int roleId)
        {
            return await _roleService.DeleteRole(roleId);
        }
    }
} 
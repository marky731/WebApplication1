using ApiLayer.Authorization;
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
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<RoleDto>>> GetRoles()
        {
            return await _roleService.GetAllRoles();
        }

        [HttpGet("{roleId}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<RoleDto>> GetSingleRole(int roleId)
        {
            return await _roleService.GetRoleById(roleId);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<RoleDto>> EditRole(RoleDto roleDto)
        {
            return await _roleService.UpdateRole(roleDto);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<RoleDto>> AddRole(RoleDto roleDto)
        {
            return await _roleService.CreateRole(roleDto);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<string?>> DeleteRole(int roleId)
        {
            return await _roleService.DeleteRole(roleId);
        }
    }
} 
using AutoMapper;
using Intermediary.Interfaces;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Services
{
    public class EfRoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public EfRoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<RoleDto>>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllAsync();
            var totalCount = await _roleRepository.GetTotalCountAsync();
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            return new ApiResponse<List<RoleDto>>(true, "Roles retrieved successfully", roleDtos);
        }

        public async Task<ApiResponse<RoleDto>> GetRoleById(int roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new Exception("Role not found");

            var roleDto = _mapper.Map<RoleDto>(role);
            return new ApiResponse<RoleDto>(true, "Role retrieved successfully", roleDto);
        }

        public async Task<ApiResponse<RoleDto>> CreateRole(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.AddAsync(role);
            await _roleRepository.SaveChangesAsync();
            return new ApiResponse<RoleDto>(true, "Role added successfully", roleDto);
        }

        public async Task<ApiResponse<RoleDto>> UpdateRole(RoleDto roleDto)
        {
            var role = await _roleRepository.GetByIdAsync(roleDto.Id);
            if (role == null)
                throw new Exception("Role not found");

            _mapper.Map(roleDto, role);
            await _roleRepository.UpdateAsync(role);
            await _roleRepository.SaveChangesAsync();
            return new ApiResponse<RoleDto>(true, "Role updated successfully", roleDto);
        }

        public async Task<ApiResponse<string?>> DeleteRole(int roleId)
        {
            await _roleRepository.DeleteAsync(roleId);
            await _roleRepository.SaveChangesAsync();
            return new ApiResponse<string?>(true, "Role deleted successfully", null);
        }
    }
} 
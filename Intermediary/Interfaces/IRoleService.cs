using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace Intermediary.Interfaces;

public interface IRoleService
{
    Task<ApiResponse<List<RoleDto>>> GetAllRoles(int pageNumber, int pageSize);
    Task<ApiResponse<RoleDto>> GetRoleById(int id);
    Task<ApiResponse<RoleDto>> CreateRole(RoleDto roleDto);
    Task<ApiResponse<RoleDto>> UpdateRole(RoleDto roleDto);
    Task<ApiResponse<string?>> DeleteRole(int id);
} 
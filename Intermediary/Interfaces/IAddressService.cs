using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace Intermediary.Interfaces;

public interface IAddressService
{
    Task<ApiResponse<List<AddressDto>>> GetAllAddresses(int pageNumber, int pageSize);
    Task<ApiResponse<AddressDto>> GetAddressById(int id);
    Task<ApiResponse<AddressDto>> CreateAddress(AddressDto addressDto);
    Task<ApiResponse<AddressDto>> UpdateAddress(AddressDto addressDto);
    Task<ApiResponse<string?>> DeleteAddress(int id);
} 
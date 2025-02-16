using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<AddressDto>>> GetAddresses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await _addressService.GetAllAddresses(pageNumber, pageSize);
        }

        [HttpGet("{addressId}")]
        public async Task<ApiResponse<AddressDto>> GetSingleAddress(int addressId)
        {
            return await _addressService.GetAddressById(addressId);
        }

        [HttpPut]
        public async Task<ApiResponse<AddressDto>> EditAddress(AddressDto addressDto)
        {
            return await _addressService.UpdateAddress(addressDto);
        }

        [HttpPost]
        public async Task<ApiResponse<AddressDto>> AddAddress(AddressDto addressDto)
        {
            return await _addressService.CreateAddress(addressDto);
        }

        [HttpDelete]
        public async Task<ApiResponse<string?>> DeleteAddress(int addressId)
        {
            return await _addressService.DeleteAddress(addressId);
        }
    }
} 
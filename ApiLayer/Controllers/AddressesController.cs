using System.Security.Claims;
using Intermediary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<List<AddressDto>>> GetAddresses()
        {
            return await _addressService.GetAllAddresses();
        }

        [HttpGet("{addressId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<AddressDto>> GetSingleAddress(int addressId)
        {
            return await _addressService.GetAddressById(addressId);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<AddressDto>> EditAddress(AddressDto addressDto)
        {
            return await _addressService.UpdateAddress(addressDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<AddressDto>> AddAddress(AddressDto addressDto)
        {
            return await _addressService.CreateAddress(addressDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<string?>> DeleteAddress(int addressId)
        {
            return await _addressService.DeleteAddress(addressId);
        }
    }
}
using AutoMapper;
using Intermediary.Interfaces;
using EntityLayer.ApiResponse;
using EntityLayer.Dtos;
using EntityLayer.Models;

namespace Intermediary.Services
{
    public class EfAddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public EfAddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<AddressDto>>> GetAllAddresses()
        {
            var addresses = await _addressRepository.GetAllAsync();
            var addressDtos = _mapper.Map<List<AddressDto>>(addresses);
            return new ApiResponse<List<AddressDto>>(true, "Addresses retrieved successfully", addressDtos);
        }

        public async Task<ApiResponse<List<AddressDto>>> GetAllPaginatedAddresses(int pageNumber, int pageSize)
        {
            var addresses = await _addressRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var addressDtos = _mapper.Map<List<AddressDto>>(addresses);
            return new ApiResponse<List<AddressDto>>(true, "Addresses retrieved successfully", addressDtos);        }

        public async Task<ApiResponse<AddressDto>> GetAddressById(int addressId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null)
                throw new Exception("Address not found");

            var addressDto = _mapper.Map<AddressDto>(address);
            return new ApiResponse<AddressDto>(true, "Address retrieved successfully", addressDto);
        }

        public async Task<ApiResponse<AddressDto>> CreateAddress(AddressDto addressDto)
        {
            var address = _mapper.Map<Address>(addressDto);
            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();
            return new ApiResponse<AddressDto>(true, "Address added successfully", addressDto);
        }

        public async Task<ApiResponse<AddressDto>> UpdateAddress(AddressDto addressDto)
        {
            var address = await _addressRepository.GetByIdAsync(addressDto.Id);
            if (address == null)
                throw new Exception("Address not found");

            _mapper.Map(addressDto, address);
            await _addressRepository.UpdateAsync(address);
            await _addressRepository.SaveChangesAsync();
            return new ApiResponse<AddressDto>(true, "Address updated successfully", addressDto);
        }

        public async Task<ApiResponse<string?>> DeleteAddress(int addressId)
        {
            await _addressRepository.DeleteAsync(addressId);
            await _addressRepository.SaveChangesAsync();
            return new ApiResponse<string?>(true, "Address deleted successfully", null);
        }
    }
} 
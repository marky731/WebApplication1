namespace EntityLayer.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public int RoleId { get; set; }
        public RoleDto Role { get; set; }
        public ICollection<AddressDto> Addresses { get; set; }
        public ICollection<ProfilePicDto> ProfilePics { get; set; }
    }
}
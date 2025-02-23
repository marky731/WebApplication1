 namespace EntityLayer.Dtos;

public class UserInfoDto
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
    public ICollection<AddressDto> Addresses { get; set; }
    public ICollection<ImageDto> ProfilePics { get; set; }
}
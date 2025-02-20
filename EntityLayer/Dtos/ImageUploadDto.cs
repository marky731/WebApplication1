using Microsoft.AspNetCore.Http;


namespace EntityLayer.Dtos;

public class ProfilePicUploadDto
{
    public int UserId { get; set; }
    public IFormFile File { get; set; }
}
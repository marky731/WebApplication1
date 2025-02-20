using Microsoft.AspNetCore.Http;


namespace EntityLayer.Dtos;

public class ImageUploadDto
{
    public int UserId { get; set; }
    public IFormFile File { get; set; }
}
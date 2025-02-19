namespace EntityLayer.Dtos;

public class ProfilePicDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ImagePath { get; set; }
    public DateTime UploadedAt { get; set; }
}
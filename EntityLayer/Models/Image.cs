namespace EntityLayer.Models;

public class Image
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ImagePath { get; set; } // Path to the image file
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    
    public virtual User User { get; set; }
}

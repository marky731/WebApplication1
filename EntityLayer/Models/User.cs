namespace EntityLayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public virtual Image Image { get; set; }
        public int? ImageId { get; set; }
        public string? PasswordHash { get; set; }  // Add this
        public string? Email { get; set; } // Add email property
    }
}
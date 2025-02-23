 namespace EntityLayer.Dtos
{
    public class UserRegisterDto
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } = 2; // Default to "User" role.  Adjust as needed.
    }
}
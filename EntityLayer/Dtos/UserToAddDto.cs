using EntityLayer.Models;

namespace EntityLayer.Dtos
{
    public class UserToAddDto
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public int RoleId { get; set; }
        public ICollection<Address> Addresses { get; set; }
        
    }
}
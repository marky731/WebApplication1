
namespace EntityLayer.Models
{
    public class Address
    {
        public int Id { get; set; } // Ensure this property is present

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int UserId { get; set; }
    }
}

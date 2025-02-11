namespace EntityLayer.Dtos
{
    public class UserToAddDto
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }

        public UserToAddDto()
        {
            
            if (Firstname == null)
            {
                Firstname = "";
            }

            if (Surname == null)
            {
                Surname = "";
            }


            if (Gender == null)
            {
                Gender = "";
            }
        }
    }
}
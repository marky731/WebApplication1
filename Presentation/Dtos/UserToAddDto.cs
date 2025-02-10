namespace Presentation.Dtos
{
    public class UserToAddDto
    {
        public string firstname { get; set; }
        public string surname { get; set; }
        public string gender { get; set; }

        public UserToAddDto()
        {
            if (firstname == null)
            {
                firstname = "";
            }

            if (surname == null)
            {
                surname = "";
            }


            if (gender == null)
            {
                gender = "";
            }
        }
    }
}
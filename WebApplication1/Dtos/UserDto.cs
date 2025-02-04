namespace WebApplication1.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string firstname {get; set;}
    public string surname {get; set;}
    public string gender {get; set;}


    public UserDto()
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
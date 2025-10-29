namespace Authentication.Dtos;

public class LoginDto(string username, string password)
{
    public string Username => username;
    public string Password => password; 
}

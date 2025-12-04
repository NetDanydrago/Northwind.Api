using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Dtos;

public class CreateUserDto(string userName, string surnames, string email, string password)
{
    public string UserName => userName;
    public string Surnames => surnames;
    public string Email => email;
    public string Password => password;
}

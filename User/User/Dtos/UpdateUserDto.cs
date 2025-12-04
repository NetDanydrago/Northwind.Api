using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Dtos;

public class UpdateUserDto(int id, string userName, string surnames, string email, string newPassword)
{
    public int Id => id;
    public string UserName => userName;
    public string Surnames => surnames;
    public string Email => email;
    public string NewPassword => newPassword;
}

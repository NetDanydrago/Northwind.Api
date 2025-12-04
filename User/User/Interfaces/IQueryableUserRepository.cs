using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Dtos;

namespace User.Interfaces;

public interface IQueryableUserRepository
{
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> GetByUserNameAsync(string userName);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetByEmailAsync(string email);

}

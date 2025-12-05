using DomainTransaction.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Dtos;

namespace User.Interfaces;
public interface ICommandUserRepository : ITransactionHandler
{
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, string passwordHash);
    Task UpdateUserAsync(UpdateUserDto dto);
    Task DeactivateUserAsync(int id);
    Task SaveChangesAsync();
}

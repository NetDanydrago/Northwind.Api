using Microsoft.EntityFrameworkCore;
using NorthWind.EFCore.Repositories.DbContexts;
using User.Dtos;
using User.Interfaces;

namespace NorthWind.EFCore.Repositories.Repositories.Users;

internal class CommandUserRepository(CommandDbContext context)
    : TransactionHandlerBase(context.Database), ICommandUserRepository
{
    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, string passwordHash)
    {
        var entity = new Entities.User
        {
            UserName = createUserDto.UserName,
            Surnames = createUserDto.Surnames,
            Email = createUserDto.Email,
            PasswordHash = passwordHash,   
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Id = entity.Id,
            UserName = entity.UserName,
            Surnames = entity.Surnames,
            Email = entity.Email,
            Password = entity.PasswordHash, 
            IsActive = entity.IsActive, 
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == updateUserDto.Id);
        if (user is null)
        {
            return;
        }

        user.UserName = updateUserDto.UserName;
        user.Surnames = updateUserDto.Surnames;
        user.Email = updateUserDto.Email;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(updateUserDto.NewPassword))
        {
            user.PasswordHash = updateUserDto.NewPassword;
        }

        context.Users.Update(user);
    }

    public async Task DeactivateUserAsync(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return;
        }

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        context.Users.Update(user);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}

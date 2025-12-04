using Microsoft.EntityFrameworkCore;
using NorthWind.EFCore.Repositories.DbContexts;
using User.Dtos;
using User.Interfaces;

namespace NorthWind.EFCore.Repositories.Repositories.Users;

internal class CommandUserRepository(CommandDbContext context)
    : TransactionHandlerBase(context.Database), ICommandUserRepository
{
    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        var passwordHash = createUserDto.Password;

        var entity = await context.Users.AddAsync(new Entities.User
        {
            UserName = createUserDto.UserName,
            Surnames = createUserDto.Surnames,
            Email = createUserDto.Email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        context.SaveChanges();

        return new UserDto
        {
            Id = entity.Entity.Id,
            UserName = entity.Entity.UserName,
            Surnames = entity.Entity.Surnames,
            Email = entity.Entity.Email,
            Password = entity.Entity.PasswordHash,
            IsActive = entity.Entity.IsActive,
            CreatedAt = entity.Entity.CreatedAt,
            UpdatedAt = entity.Entity.UpdatedAt
        };
    }

    public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == updateUserDto.Id);
        if (user != null)
        {
            user.UserName = updateUserDto.UserName;
            user.Surnames = updateUserDto.Surnames;
            user.Email = updateUserDto.Email;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(updateUserDto.NewPassword))
            {
                // user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.NewPassword);
                user.PasswordHash = updateUserDto.NewPassword;
            }

            context.Users.Update(user);
        }
    }

    public async Task DeactivateUserAsync(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            context.Users.Update(user);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}

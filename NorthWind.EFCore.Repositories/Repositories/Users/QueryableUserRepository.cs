using Microsoft.EntityFrameworkCore;
using NorthWind.EFCore.Repositories.DbContexts;
using User.Dtos;
using User.Interfaces;

namespace NorthWind.EFCore.Repositories.Repositories.Users;

internal class QueryableUserRepository(QueryDbContext context) : IQueryableUserRepository
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var query = context.Users.Select(s => new UserDto
        {
            Id = s.Id,
            UserName = s.UserName,
            Surnames = s.Surnames,
            Email = s.Email,
            Password = s.PasswordHash,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        });

        return await query.ToListAsync();
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var query = context.Users
            .Where(u => u.Email == email)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Surnames = u.Surnames,
                Email = u.Email,
                Password = u.PasswordHash,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            });

        return await query.FirstOrDefaultAsync();
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var query = context.Users
            .Where(w => w.Id == id)
            .Select(s => new UserDto
            {
                Id = s.Id,
                UserName = s.UserName,
                Surnames = s.Surnames,
                Email = s.Email,
                Password = s.PasswordHash,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            });

        return await query.FirstOrDefaultAsync();
    }

    public async Task<UserDto> GetByUserNameAsync(string userName)
    {
        var query = context.Users
            .Where(w => w.UserName == userName)
            .Select(s => new UserDto
            {
                Id = s.Id,
                UserName = s.UserName,
                Surnames = s.Surnames,
                Email = s.Email,
                Password = s.PasswordHash,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            });

        return await query.FirstOrDefaultAsync();
    }
}

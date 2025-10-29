namespace Authentication.Interfaces;

public interface IQueryableAuthenticationUsersRepository
{
    Task<UserDto> GetByUserIdAsync(int id);
    Task<UserDto> GetByUsernameAsync(string username);
}

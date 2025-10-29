namespace Authentication.Dtos;

public class UserDto(int id, string userName,string fullName,string email,string passwordHash, bool isActive, string role, DateTime createdAt, DateTime lastLoginAt)
{
    public int Id => id;
    public string Username  => userName;
    public string FullName => fullName;
    public string Email => email;
    public string PasswordHash  => passwordHash;
    public bool IsActive => isActive;
    public string Role => role;
    public DateTime CreatedAt => createdAt;
    public DateTime? LastLoginAt => lastLoginAt;
}

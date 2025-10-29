namespace Authentication.Services;

internal class PasswordService(ILogger<PasswordService> logger) : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        bool Result = false;
        try
        {
            Result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error verifying password");
        }
        return Result;
    }
}

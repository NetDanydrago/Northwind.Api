namespace Authentication.Services;
internal class PasswordPlainService(ILogger<PasswordPlainService> logger) : IPasswordService
{
    public string HashPassword(string password)
    {
        return password;
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        bool Result = false;
        try
        {
            Result = password == hashedPassword;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error verifying password");
        }
        return Result;
    }
}

using Microsoft.Extensions.Options;

namespace Authentication.Handler;
internal class AuthenticationRecoveryPasswordHandler(ICommandAuthenticationUserRepository commandUser, IQueryableAuthenticationUsersRepository usersRepository, ILogger<AuthenticationRecoveryPasswordHandler> logger) : IAuthenticationRecoveryPasswordInputPort
{
    public async Task RecoveryPasswordAsync(RecoveryPasswordDto recoveryPasswordDto)
    {
        try
        {
            logger.LogInformation("Starting password recovery process for username: {username}", recoveryPasswordDto.Username);
            var user = await usersRepository.GetByUsernameAsync(recoveryPasswordDto.Username);
            if (user == null)
            {
                throw new Exception(AuthenticationMessages.UserNotFound);
            }
            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(commandUser);
            string generatePassword = GeneratePassword();
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(generatePassword, BCrypt.Net.BCrypt.GenerateSalt(12));
            await commandUser.UpdatePasswordUserAsync(user.Id, hashPassword);
            await commandUser.SaveChangesAsync();
            scope.Complete();
            logger.LogInformation("Password recovery process initiated for user with username: {username}", recoveryPasswordDto.Username);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during password recovery process for username: {username}", recoveryPasswordDto.Username);
            throw new Exception(AuthenticationMessages.RecoveryPasswordError, ex);
        }
    }

    private string GeneratePassword()
    {
        var random = new Random();
        var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        // Generate two uppercase letters
        var firstTwo = new string(Enumerable.Range(0, 2)
            .Select(_ => upperChars[random.Next(upperChars.Length)])
            .ToArray());

        // Generate remaining lowercase characters and numbers
        var remaining = new string(Enumerable.Range(0, 6)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());

        return $"{firstTwo}{remaining}$$";
    }

}

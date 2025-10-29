namespace Authentication.Interfaces;
public interface ICommandAuthenticationUserRepository : ITransactionHandler
{
    Task UpdatePasswordUserAsync(int UserId, string newPassword);

    Task SaveChangesAsync();
}

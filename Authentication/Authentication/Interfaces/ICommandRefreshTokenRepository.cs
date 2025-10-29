using Authentication.Dtos;

namespace Authentication.Interfaces;

public interface ICommandRefreshTokenRepository : ITransactionHandler
{
    Task CreateAsync(RefreshTokenDto refreshToken);
    Task UpdateAsync(RefreshTokenDto refreshToken);
    Task RevokeTokenAsync(string token);
    Task SaveChangesAsync();
}

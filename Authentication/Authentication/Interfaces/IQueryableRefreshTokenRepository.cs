using Authentication.Dtos;

namespace Authentication.Interfaces;
public interface IQueryableRefreshTokenRepository
{
    Task<RefreshTokenDto> GetByTokenAsync(string token);
}

using Microsoft.Extensions.Logging;
using User.Dtos;
using User.Interfaces;
using User.Internals.InputPorts;

namespace User.Handler;

internal class GetAllUsersHandler(
    IQueryableUserRepository queryableUserRepository,
    ILogger<GetAllUsersHandler> logger) : IGetAllUsersInputPort
{
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        try
        {
            logger.LogInformation("Attempting to get all active users");

            var users = await queryableUserRepository.GetAllUsersAsync();

            logger.LogInformation("Successfully retrieved {Count} active users", users.Count());

            return users;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all active users");
            throw;
        }
    }
}

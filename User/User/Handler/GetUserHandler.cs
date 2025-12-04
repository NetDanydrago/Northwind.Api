using Microsoft.Extensions.Logging;
using User.Dtos;
using User.Interfaces;
using User.Internals.InputPorts;
using User.Resources;

namespace User.Handler;

internal class GetUserHandler(
    IQueryableUserRepository queryableUserRepository,
    ILogger<GetUserHandler> logger) : IGetUserInputPort
{
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Attempting to get user by ID: {Id}", id);

            var user = await queryableUserRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception(UserMessager.UserNotFound);
            }

            logger.LogInformation("Successfully retrieved user: {Id}", id);

            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user by ID: {Id}", id);
            throw;
        }
    }
}

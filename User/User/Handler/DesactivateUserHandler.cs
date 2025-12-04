using DomainTransaction;
using Microsoft.Extensions.Logging;
using User.Dtos;
using User.Interfaces;
using User.Internals.InputPorts;
using User.Resources;

namespace User.Handler;

internal class DesactivateUserHandler(
    ICommandUserRepository userRepository,
    IQueryableUserRepository queryableUserRepository,
    ILogger<DesactivateUserHandler> logger) : IDesactivateUserInputPort
{
    public async Task DesactivateUserAsync(DesactivateUserDto desactivateUserDto)
    {
        try
        {
            logger.LogInformation("Attempting to deactivate user: {Id}", desactivateUserDto.Id);

            var existingUser = await queryableUserRepository.GetByIdAsync(desactivateUserDto.Id);
            if (existingUser == null)
            {
                throw new Exception(UserMessager.UserNotFound);
            }

            if (!existingUser.IsActive)
            {
                throw new Exception(UserMessager.UserAlReadyInactive);
            }

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(userRepository);

            await userRepository.DeactivateUserAsync(desactivateUserDto.Id);
            await userRepository.SaveChangesAsync();

            scope.Complete();

            logger.LogInformation("Successfully deactivated user: {Id}", desactivateUserDto.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deactivating user: {Id}", desactivateUserDto.Id);
            throw;
        }
    }
}

using DomainTransaction;
using Microsoft.Extensions.Logging;
using User.Dtos;
using User.Helpers;
using User.Interfaces;
using User.Internals.InputPorts;
using User.Resources;

namespace User.Handler;

internal class UpdateUserHandler(
    ICommandUserRepository userRepository,
    IQueryableUserRepository queryableUserRepository,
    ILogger<UpdateUserHandler> logger) : IUpdateUserInputPort
{
    public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        try
        {
            logger.LogInformation("Attempting to update user: {Id}", updateUserDto.Id);

            var existUser = await queryableUserRepository.GetByIdAsync(updateUserDto.Id);
            if (existUser == null)
                throw new Exception(UserMessager.UserNotFound);

            var userWithSameEmail = await queryableUserRepository.GetByEmailAsync(updateUserDto.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != updateUserDto.Id)
                throw new Exception(UserMessager.EmailAlreadyExists);

            var userWithSameUserName = await queryableUserRepository.GetByUserNameAsync(updateUserDto.UserName);
            if (userWithSameUserName != null && userWithSameUserName.Id != updateUserDto.Id)
                throw new Exception(UserMessager.UserNameAlreadyExists);

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(userRepository);

            if (!string.IsNullOrWhiteSpace(updateUserDto.NewPassword))
            {
                var hashedPassword = PasswordHelper.Hash(updateUserDto.NewPassword);
                updateUserDto = new UpdateUserDto(
                    updateUserDto.Id,
                    updateUserDto.UserName,
                    updateUserDto.Surnames,
                    updateUserDto.Email,
                    hashedPassword
                );
            }

            await userRepository.UpdateUserAsync(updateUserDto);
            await userRepository.SaveChangesAsync();

            scope.Complete();

            logger.LogInformation("User updated successfully: {Id}", updateUserDto.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user: {Id}", updateUserDto.Id);
            throw;
        }
    }
}

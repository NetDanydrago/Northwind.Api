using DomainTransaction;
using Microsoft.Extensions.Logging;
using User.Dtos;
using User.Internals.InputPorts;
using User.Interfaces;
using User.Resources;

namespace User.Handler;

internal class CreateUserHandler(
    ICommandUserRepository userRepository,
    IQueryableUserRepository queryUserRepository,
    ILogger<CreateUserHandler> logger) : ICreateUserInputPort
{
    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        try
        {
            logger.LogInformation("Attempting to create user: {Email}", createUserDto.Email);

            // 1️⃣ Validar correo duplicado
            var existingByEmail = await queryUserRepository.GetByEmailAsync(createUserDto.Email);
            if (existingByEmail != null)
            {
                throw new Exception(UserMessager.EmailAlreadyExists);
            }

            // 2️⃣ (Opcional) validar username duplicado
            var existingByUserName = await queryUserRepository.GetByUserNameAsync(createUserDto.UserName);
            if (existingByUserName != null)
            {
                throw new Exception(UserMessager.UserNameAlreadyExists);
            }

            var newUser = await userRepository.CreateUserAsync(createUserDto);
            await userRepository.SaveChangesAsync();

            return newUser;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user: {Email}", createUserDto.Email);
            throw;
        }
    }
}

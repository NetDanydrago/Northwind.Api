using DomainTransaction;
using Microsoft.Extensions.Logging;
using User.Dtos;
using User.Internals.InputPorts;
using User.Interfaces;
using User.Resources;
using User.Helpers;

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

            //  Validar correo duplicado
            var existingByEmail = await queryUserRepository.GetByEmailAsync(createUserDto.Email);
            if (existingByEmail != null)
            {
                throw new Exception(UserMessager.EmailAlreadyExists);
            }

            // validar username duplicado
            var existingByUserName = await queryUserRepository.GetByUserNameAsync(createUserDto.UserName);
            if (existingByUserName != null)
            {
                throw new Exception(UserMessager.UserNameAlreadyExists);
            }

            // Hashear contraseña 
            var passwordHash = PasswordHelper.Hash(createUserDto.Password);

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(userRepository);

            var newUser = await userRepository.CreateUserAsync(createUserDto, passwordHash);
            await userRepository.SaveChangesAsync();

            scope.Complete();

            logger.LogInformation("User created successfully: {Email} (Id: {Id})",
                newUser.Email, newUser.Id);

            return newUser;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user: {Email}", createUserDto.Email);
            throw;
        }
    }
}

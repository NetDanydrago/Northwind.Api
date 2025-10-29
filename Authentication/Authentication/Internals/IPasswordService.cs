namespace Authentication.Internals;

internal interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

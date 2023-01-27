namespace AuthenticationServer.API.Service.PasswordHasher
{
    public interface IPasswordHasher
    {
        string HashPassword(string Password);
        bool VerifyPassword(string Password,string passwordHash);
    }
}

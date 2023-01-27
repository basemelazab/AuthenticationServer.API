namespace AuthenticationServer.API.Service.PasswordHasher
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password);
        }

        public bool VerifyPassword(string Password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(Password,passwordHash);
        }
    }
}

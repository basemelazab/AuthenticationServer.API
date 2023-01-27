using AuthenticationServer.API.Model;

namespace AuthenticationServer.API.Service.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        public readonly AuthenticationConfiguration _configuration;
        public readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {
            return _tokenGenerator.GenerateToken(
                    _configuration.RefreshTokenSecret,
                    _configuration.Issuer,
                    _configuration.Audience,
                    _configuration.RefreshTokenExpirationMinutes);
        }
    }
}

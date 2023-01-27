using AuthenticationServer.API.Model;
using AuthenticationServer.API.Model.Requests;
using AuthenticationServer.API.Model.Response;
using AuthenticationServer.API.Model.Users;
using AuthenticationServer.API.Service.PasswordHasher;
using AuthenticationServer.API.Service.RefreshTokenRepositories;
using AuthenticationServer.API.Service.TokenGenerators;

namespace AuthenticationServer.API.Service.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public Authenticator(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticatedUserResponse> Authenticated(User user)
        {
         
            string accessToken = _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();
            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id,
            };
            await _refreshTokenRepository.Create(refreshTokenDTO);
            return new AuthenticatedUserResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}

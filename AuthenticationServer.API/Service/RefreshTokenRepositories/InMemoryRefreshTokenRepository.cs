using AuthenticationServer.API.Model;

namespace AuthenticationServer.API.Service.RefreshTokenRepositories
{
    public class InMemoryRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly List<RefreshToken> _refreshToken=new List<RefreshToken>();
        public Task Create(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid();
            _refreshToken.Add(refreshToken);
            return Task.CompletedTask;
        }

        public Task Delete(Guid id)
        {
            _refreshToken.RemoveAll(x => x.Id == id);
            return Task.CompletedTask;
        }

        public Task DeleteAll(Guid userId)
        {
            _refreshToken.RemoveAll(x=>x.UserId==userId);
            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetByToken(string token)
        {
           RefreshToken refreshToken =_refreshToken.FirstOrDefault(x=>x.Token==token);
           return Task.FromResult(refreshToken);
        }
    }
}

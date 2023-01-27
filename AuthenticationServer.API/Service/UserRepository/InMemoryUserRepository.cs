using AuthenticationServer.API.Model.Users;

namespace AuthenticationServer.API.Service.UserRepository
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users= new List<User>();
        public Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(x=>x.Email==email));
        }

        public Task<User> GetById(Guid userId)
        {
            return Task.FromResult(_users.FirstOrDefault(x => x.Id == userId));
        }

        public Task<User> GetByUserName(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(x=>x.UserName==username));
        }
    }
}

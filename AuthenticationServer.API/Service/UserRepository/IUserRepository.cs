using AuthenticationServer.API.Model.Users;

namespace AuthenticationServer.API.Service.UserRepository
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUserName(string username);
        Task<User> Create(User user);
        Task<User> GetById(Guid userId);
    }
}

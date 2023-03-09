using Shared;
using Shared.DTOs;

namespace Application.DAOInterface;

//DAO classes will support CRUD operations
public interface IUserDao
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters);
    Task<User?> GetByIdAsync(int id);
    
}
using Application.DAOInterface;
using Shared;
using Shared.DTOs;

namespace FileData.DAOs;

public class UserFileDao:IUserDao
{
    
    private readonly FileContext context;

    public UserFileDao(FileContext context)
    {
        this.context = context;
    }
    

    public Task<User> CreateAsync(User user)
    {
        int userId = 1;//if there is no user then id is set to 1
        if (context.Users.Any())
        {//if any user then  max method will look through all user and then return max value from the property Id.
            userId = context.Users.Max(u => u.Id);
            userId++;
        }

        user.Id = userId;
        context.Users.Add(user);
        context.SaveChanges();

        return Task.FromResult(user);
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        //first or default will find the first matching object the criteria specified and if nothing found then return null
        var existing =
            context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(existing);
    }

    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        //AsEnumerable converts ICollection to IEnumerable 6
        IEnumerable<User> users = context.Users.AsEnumerable();
        if (searchParameters.UsernameContains!=null)
        {
            users = context.Users.Where(u =>
                u.Username.Contains(searchParameters.UsernameContains, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(users);
    }

    public Task<User?> GetByIdAsync(int id)
    {
        User? existing = context.Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(existing);
    }
}
using Application.DAOInterface;
using Application.LogicInterfaces;
using Shared;
using Shared.DTOs;

namespace Application.Logic;

public class UserLogic: IUserLogic
{
    private readonly IUserDao userDao;

    public UserLogic(IUserDao userDao)
    {
        this.userDao = userDao;
    }

    public async Task<User> CreateAsync(UserCreationDto userToCreate)
    {
        User? existing = await userDao.GetByUsernameAsync(userToCreate.Username);//check if the username is taken
        if (existing!=null)
        {
            throw new Exception("Username already taken"); //exception is caught in the controller class
        }

        ValidateData(userToCreate);//checks the rules of the username
        User toCreate = new User()
        {
            Username = userToCreate.Username
        };
        User created = await userDao.CreateAsync(toCreate);//new user is created and handed over to dao for storage
        return created;
    }


    private static void ValidateData(UserCreationDto userToCreate)
    {
        string username = userToCreate.Username;

        if (username.Length < 3)
        {
            throw new Exception("Username must be at least 3 characters");
        }

        if (username.Length>15)
        {
            throw new Exception("Username should be less than 16 characters");
        }
    }

    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        return userDao.GetAsync(searchParameters);//dont need to wait it because we donot need the result here..
    }
    
}
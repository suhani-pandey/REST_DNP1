using Shared;
using Shared.DTOs;

namespace Application.LogicInterfaces;

public interface IUserLogic
{
     //Task because we want to do some work asynchronously  
     Task<User> CreateAsync(UserCreationDto userToCreate);
     Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters);
}
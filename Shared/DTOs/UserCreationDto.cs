namespace Shared.DTOs;

//all layers will know about DTOs and domain objects;there is dependancy so if we change DTOs then all layers are affected
//  

public class UserCreationDto
{
    //As user class contains more properties than the what we needed to create a new class , we instead create a new DTO with just properties
    //we need to create a new user
    public string Username { get; }

    public UserCreationDto(string username)
    {
        Username = username;
    }
}
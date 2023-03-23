using System.Net.Http.Json;
using System.Text.Json;
using HTTPClients.ClientInterfaces;
using Shared;
using Shared.DTOs;

namespace HTTPClients.Implementation;

public class UserHttpClient : IUserService
{
    //request HTTPclient through the contructor 
    //leaving the creation of the httpclient up to the blazor framework
    private readonly HttpClient client;

    public UserHttpClient(HttpClient client)
    {
        this.client = client;
    }

    //async so that we cannot wait anything in the method body--
    public async Task<User> Create(UserCreationDto dto)
    {
        //use the client to make post request to /users and send dto
        //dto will be serialized and then wrapped in the string content object
        HttpResponseMessage response = await client.PostAsJsonAsync("/user", dto);
        string
            result = await response.Content
                .ReadAsStringAsync(); //request returns the response whether we actually expect object or not
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        User? user = JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return user;
    }

    public async Task<IEnumerable<User>> GetUsers(string? usernameContains = null)
    {
        //construct URI
        //make request
        //check the response
        //deserialize the response
        //return it
        string uri = "/user";
        if (!string.IsNullOrEmpty(usernameContains))
        {
            uri += $"?username={usernameContains}";
        }

        HttpResponseMessage responseMessage = await client.GetAsync(uri);
        string result = await responseMessage.Content.ReadAsStringAsync();
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        IEnumerable<User> users = JsonSerializer.Deserialize<IEnumerable<User>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return users;

    }
}
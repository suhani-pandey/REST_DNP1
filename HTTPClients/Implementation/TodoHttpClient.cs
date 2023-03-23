using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HTTPClients.ClientInterfaces;
using Shared;
using Shared.DTOs;

namespace HTTPClients;

public class TodoHttpClient:ITodoService
{
    private readonly HttpClient client;

    public TodoHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task CreateAsync(TodoCreationDto dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/Todo", dto);
       

        if (!response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            throw new Exception(result);
        }
    }

    //making a GET request, checking the status code, and deserializing the response
    public async Task<ICollection<Todo>> GetAsync(string? userName, int? userId, bool? completedStatus, string? titleContains)
    {
        string query = ConstructQuery(userName, userId, completedStatus, titleContains);

        HttpResponseMessage response = await client.GetAsync("/Todo"+query);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        ICollection<Todo> todos = JsonSerializer.Deserialize<ICollection<Todo>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return todos;
    }

    private static string ConstructQuery(string? userName, int? userId, bool? completedStatus, string? titleContains)
    {//check all the filter argument and check if they are null, in that case they should be ignored otherwise include the needed filter
        string query = "";
        if (!string.IsNullOrEmpty(userName))
        {
            //query must always start with "?" and separated with "&"
            query += $"?username={userName}";
        }
        if (userId != null)
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"userid={userId}";
        }

        if (completedStatus != null)
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"completedstatus={completedStatus}";
        }

        if (!string.IsNullOrEmpty(titleContains))
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"titlecontains={titleContains}";
        }

        return query;
    }

    //make the update request
    //must make the patch request snd check the response for error code
    public async Task UpdateAsync(TodoUpdateDto dto)
    {
        string dtoAsJson = JsonSerializer.Serialize(dto);//first serialized to JSON
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");//create stringcontent to hold the data , we are sending JSON so we pass application/json
        

        HttpResponseMessage response = await client.PatchAsync("/todo", body);//make patch request to the client
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task<TodoBasicDto> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"/todo{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        TodoBasicDto todo = JsonSerializer.Deserialize<TodoBasicDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return todo;
    }

    
}
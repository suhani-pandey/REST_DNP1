using System.Text.Json.Serialization;

namespace Shared;

public class User
{

    public int Id { get; set; }
    public string Username { get; set; }
    public List<Todo> Type { get; set; }

    [JsonIgnore]
    public ICollection<Todo> Todos { get; set; }
}
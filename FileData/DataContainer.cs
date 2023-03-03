using Shared;

namespace FileData;

public class DataContainer
{
    //read data from the file and load this into these two collections
    public ICollection<User> Users { get; set; }
    public ICollection<Todo> Todos { get; set; }
}
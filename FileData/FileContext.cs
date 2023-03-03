using System.Text.Json;
using Shared;

namespace FileData;

public class FileContext
{
    private const string filePath = "data.json"; //this define path for the file
    
    //DataContainer will keep all our data after being loaded
    private DataContainer? DataContainer { get; set; }

    public ICollection<Todo> Todos
    {
        get
        {
            LoadData();//check if the data is loaded . if datacontainer is null then the data is loaded.
            return DataContainer!.Todos;
        }
    }

    public ICollection<User> Users
    {
        get
        {
            LoadData();
            return DataContainer!.Users;
        }
    }

    //The method is private because this class should be responsible for determining when to load data.
    private void LoadData()
    {
        /*
         This is also one of the way to do --
         if (DataContainer==null)
        {
            string content = File.ReadAllText(filePath);
            DataContainer = JsonSerializer.Deserialize<DataContainer>(content);
        }
        */
        if (DataContainer != null) return;//check if the data is already loaded
    
        if (!File.Exists(filePath))
        {
            DataContainer = new ()
            {
                Todos = new List<Todo>(),
                Users = new List<User>()
            };
            return;
        }
        string content = File.ReadAllText(filePath);
        DataContainer = JsonSerializer.Deserialize<DataContainer>(content);
    }

    //The method is to take the content of the DataContainer field, and put into the file.
    // call SaveChanges after interacting with the database.
    //DataContainer is serialized  to JSON then written to the file then field is cleared.
    public void SaveChanges()
    {
        string serialized = JsonSerializer.Serialize(DataContainer);
        File.WriteAllText(filePath, serialized);
        DataContainer = null;
    }
}
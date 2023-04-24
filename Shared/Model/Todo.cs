using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class Todo
{
    public int Id { get; set; }
    [NotMapped]
    public User Owner { get; private set; }
    
    [MaxLength(50)]
    public string Title { get; private set; }
    public bool IsCompleted { get; set; }

    public Todo( User owner, string title)
    {
        Owner = owner;
        Title = title;
    }
    
    private Todo(){}
}

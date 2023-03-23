using Application.DAOInterface;
using Shared;
using Shared.DTOs;

namespace FileData.DAOs;

public class TodoFileDao:ITodoDao
{

    private readonly FileContext context;

    public TodoFileDao(FileContext context)
    {
        this.context = context;
    }

    public Task<Todo> CreateAsync(Todo todo)
    {
        int id = 1;
        if (context.Todos.Any())
        {
            id = context.Todos.Max(t => t.Id);
            id++;
        }

        todo.Id = id;
        context.Todos.Add(todo);
        context.SaveChanges();

        return Task.FromResult(todo);
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchParameters)
    {
        IEnumerable<Todo> result = context.Todos.AsEnumerable();

        if (!string.IsNullOrEmpty(searchParameters.Username))
        {
            result = context.Todos.Where(todo =>
                todo.Owner.Username.Equals(searchParameters.Username, StringComparison.OrdinalIgnoreCase));
        }

        if (searchParameters.UserId != null)
        {
            result = result.Where(t => t.Owner.Id == searchParameters.UserId);
        }

        if (searchParameters.CompletedStatus != null)
        {
            result = result.Where(t => t.IsCompleted == searchParameters.CompletedStatus);
        }

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
        {
            result = result.Where(t =>
                t.Title.Contains(searchParameters.TitleContains, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(result);
    }

    public Task<Todo> GetByIdAsync(int id)
    {
        var existing = context.Todos.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(existing);
    }

    public Task UpdateAsync(Todo todo)
    {
        var existing = context.Todos.FirstOrDefault(t => t.Id == todo.Id);

        if (existing==null)
        {
            throw new Exception($"Todo with id {todo.Id} does not exist");
        }

        context.Todos.Remove(existing);
        context.Todos.Add(todo);
        
        context.SaveChanges();

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        var existing = context.Todos.FirstOrDefault(todo => todo.Id == id);
        if (existing == null)
        {
            throw new Exception($"Todo with id {id} does not exist!");
        }

        context.Todos.Remove(existing); 
        context.SaveChanges();
    
        return Task.CompletedTask;
    }
}
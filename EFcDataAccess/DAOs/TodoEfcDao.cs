using Application.DAOInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared;
using Shared.DTOs;

namespace EFcDataAccess.DAOs;

public class TodoEfcDao : ITodoDao
{
    
    private readonly TodoContext context;

    public TodoEfcDao(TodoContext context)
    {
        this.context = context;
    }
    
    public async Task<Todo> CreateAsync(Todo todo)
    {
       EntityEntry<Todo> createTodo= await context.Todos.AddAsync(todo);
       await context.SaveChangesAsync();
       return createTodo.Entity;

    }

    public async Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchParameters)
    {
        IQueryable<Todo> query = context.Todos.Include(todo => todo.Owner).AsQueryable();
    
        if (!string.IsNullOrEmpty(searchParameters.Username))
        {
            query = query.Where(todo =>
                todo.Owner.Username.ToLower().Equals(searchParameters.Username.ToLower()));
        }
    
        if (searchParameters.UserId != null)
        {
            query = query.Where(t => t.Owner.Id == searchParameters.UserId);
        }
    
        if (searchParameters.CompletedStatus != null)
        {
            query = query.Where(t => t.IsCompleted == searchParameters.CompletedStatus);
        }
    
        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
        {
            query = query.Where(t =>
                t.Title.ToLower().Contains(searchParameters.TitleContains.ToLower()));
        }

        List<Todo> result = await query.ToListAsync();
        return result;
    }
    

    public async Task UpdateAsync(Todo todo)
    {
        context.Todos.Update(todo);
        await context.SaveChangesAsync();

    }

    public async Task<Todo> GetByIdAsync(int id)
    {
        Todo? found = await context.Todos
            .AsNoTracking()
            .Include(todo => todo.Owner)
            .SingleOrDefaultAsync(todo => todo.Id == id);
        return found;
    }

    public async Task DeleteAsync(int id)
    {
        Todo? existing = await GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Todo with id {id} not found");
        }

        context.Todos.Remove(existing);
        await context.SaveChangesAsync();
    }
}
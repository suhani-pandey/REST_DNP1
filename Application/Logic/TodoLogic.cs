using System.Collections;
using Application.DAOInterface;
using Application.LogicInterfaces;
using Shared;
using Shared.DTOs;

namespace Application.Logic;

public class TodoLogic:ITodoLogic
{
    private readonly ITodoDao todoDao;
    private readonly IUserDao userDao;

    public TodoLogic(ITodoDao todoDao, IUserDao userDao)
    {
        this.todoDao = todoDao;
        this.userDao = userDao;
    }

    public async Task<Todo> CreateAsync(TodoCreationDto dto)
    {
        User? user = await userDao.GetByIdAsync(dto.OwnerId);
        if (user == null)
        {
            throw new Exception($"User with id {dto.OwnerId} was not found.");
        }

        ValidateTodo(dto);
        Todo todo = new Todo(user, dto.Title);
        Todo created = await todoDao.CreateAsync(todo);
        return created;
    }

    private void ValidateTodo(TodoCreationDto dto)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new Exception("Title cannot be empty");
        
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchParameters)
    {
        return todoDao.GetAsync(searchParameters);
    }

    public async Task UpdateAsync(TodoUpdateDto todo)
    {
        var existing = await todoDao.GetByIdAsync(todo.Id);

        if (existing==null)
        {
            throw new Exception($"Todo with ID {todo.Id} not found!");
        }

        User? user = null;
        if (todo.OwnerId!=null)
        {
            user = await userDao.GetByIdAsync((int)todo.OwnerId);
            if (user==null)
            {
                throw new Exception($"User with id{todo.OwnerId} was not found");
            }
        }

        if (todo.IsCompleted != null && existing.isCompleted && !(bool)todo.IsCompleted)
        {
            throw new Exception("Cannot un-complete a completed Todo");
        }

        User userToUse = user ?? existing.Owner;
        string titleToUse = todo.Title ?? existing.Title;
        bool completedToUse = todo.IsCompleted ?? existing.isCompleted;
        
        Todo updated = new (userToUse, titleToUse)
        {
            isCompleted = completedToUse,
            Id = existing.Id,
        };

        ValidateTodo(updated);

        await todoDao.UpdateAsync(updated);
    }

    private void ValidateTodo(Todo dto)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new Exception("Title cannot be empty.");
        // other validation stuff
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await todoDao.GetByIdAsync(id);
        if (existing==null)
        {
            throw new Exception($"Todo with {id} was not found");
        }
        if (existing.isCompleted==false)
        {
            throw new Exception($"cannot delete it");
            
        }
        await todoDao.DeleteAsync(id);
    }
}
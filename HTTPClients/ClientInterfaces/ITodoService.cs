using Shared;
using Shared.DTOs;

namespace HTTPClients.ClientInterfaces;

public interface ITodoService
{
    
        Task CreateAsync(TodoCreationDto dto);
        Task<ICollection<Todo>> GetAsync(string? userName, int? userId, bool? completedStatus, string? titleContains);

        Task UpdateAsync(TodoUpdateDto dto);

        Task<TodoBasicDto> GetByIdAsync(int id);
        

}
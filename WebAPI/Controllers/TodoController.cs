using Application.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;

namespace WenAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class TodoController:ControllerBase
{
    private readonly ITodoLogic todoLogic;

    public TodoController(ITodoLogic todoLogic)
    {
        this.todoLogic = todoLogic;
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateAsync([FromBody] TodoCreationDto dto)
    {
        try
        {
            Todo created = await todoLogic.CreateAsync(dto);
            return Created($"/todos/{created.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetAsync([FromQuery] string? userName, [FromQuery] int? userId,
        [FromQuery] bool? completedStatus, [FromQuery] string? titleContains)
    {
        try
        {
            SearchTodoParametersDto parametersDto=new(userName, userId, completedStatus, titleContains);
            var todos = await todoLogic.GetAsync(parametersDto);
            return Ok(todos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    
    [HttpGet]
    [Route("{todoId:int}")]
    public async Task<ActionResult<Todo>> GetTodoByIdAsync([FromRoute] int todoId)
    {
        try
        {
            Todo dto = await todoLogic.GetByIdAsync(todoId);
            return Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    

    [HttpPatch]
    public async Task<ActionResult> UpdateAsync([FromBody] TodoUpdateDto updateDto)
    {
        try
        {
            await todoLogic.UpdateAsync(updateDto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete ("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await todoLogic.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}
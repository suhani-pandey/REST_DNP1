using Application.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;

namespace WenAPI.Controllers;


[ApiController]//marks the class as api controller so that web api framework will know about this class
[Route("[controller]")]//specifies the sub URI to access this controller class
public class UserController:ControllerBase
{

    private readonly IUserLogic userLogic;

    public UserController(IUserLogic userLogic)
    {
        this.userLogic = userLogic;
    }

    [HttpPost]//post request to this user should hit this endpoint
    public async Task<ActionResult<User>> CreateAsync(UserCreationDto dto)
    {
        try
        {
            User user = await userLogic.CreateAsync(dto);
            return Created($"/users{user.Id}", user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500,e.Message);
        }
    }

    [HttpGet]//GET requests to this controller ends here
    //FromQuery to indicate that this argument should be extracted from the query parameters of the URI.
    public async Task<ActionResult<IEnumerable<User>>> GetAsync([FromQuery] string? username)
    {
        try
        {
            SearchUserParametersDto parameters = new(username);
            IEnumerable<User> users = await userLogic.GetAsync(parameters);
            return Ok(users);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
} 
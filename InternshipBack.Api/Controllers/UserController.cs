using InternshipBack.Core.Commands.User;
using Microsoft.AspNetCore.Mvc;
using InternshipBack.Core.Queries.User;

namespace InternshipBack.api.Controllers;

public class UserController : BaseController
{
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    
}
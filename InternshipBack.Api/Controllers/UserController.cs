using Microsoft.AspNetCore.Mvc;
using InternshipBack.Core.Queries;

namespace InternshipBack.api.Controllers;

public class UserController : BaseController
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    
}
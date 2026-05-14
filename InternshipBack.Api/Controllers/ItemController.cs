using Microsoft.AspNetCore.Mvc;
using InternshipBack.Core.Commands;
using InternshipBack.Core.Queries;

namespace InternshipBack.api.Controllers;

public class ItemController : BaseController
{
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateItemCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("GetAllFiltered")]
    public async Task<IActionResult> GetAllFiltered([FromQuery] GetAllItemsFilteredQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(DeleteItemCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    
    
}
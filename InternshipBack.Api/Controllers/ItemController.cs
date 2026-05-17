using InternshipBack.Core.Commands.Item;
using Microsoft.AspNetCore.Mvc;
using InternshipBack.Core.Queries.Item;

namespace InternshipBack.Api.Controllers;

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
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteItemCommand
        {
            Id = id
        });
        
        return Ok(result);
    }
    
    [HttpPost("ExportPdf")]
    public async Task<IActionResult> ExportPdf([FromBody]ExportItemsToPdfQuery  query)
    {
        var pdf = await Mediator.Send(query);
        return File(pdf, "application/pdf", "items-export.pdf");
    }
        
}
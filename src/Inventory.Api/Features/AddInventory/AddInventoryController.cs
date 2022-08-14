using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Features.AddInventory;

[ApiController]
public class AddInventoryController : ControllerBase
{
    private readonly IAddInventoryService _service;

    public AddInventoryController(IAddInventoryService service)
    {
        _service = service;
    }
    
    [HttpPost("api/inventory")]
    public async Task<IActionResult> AddAsync([FromBody] AddInventoryRequest request)
    {
        var operationResult = await _service.ExecuteAsync(request);
        if (operationResult.IsSuccessful)
        {
            return Accepted();
        }

        return StatusCode((int) (HttpStatusCode.InternalServerError));
    }
}
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Features.AddInventory;

[ApiController]
public class Controller : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddInventoryAsync(AddInventoryRequest request)
    {
        throw new NotImplementedException();
    }
}
namespace Inventory.Api.Features.AddInventory;

public class AddInventoryRequest
{
    public string CorrelationId { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string LocationCode { get; set; }
}


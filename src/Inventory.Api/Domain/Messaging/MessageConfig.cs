namespace Inventory.Api.Domain.Messaging;

public class MessageConfig
{
    public string ConnectionString { get; set; }
    public string QueueName { get; set; }
}
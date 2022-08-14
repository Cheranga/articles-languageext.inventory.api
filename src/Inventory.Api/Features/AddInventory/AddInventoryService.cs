using Inventory.Api.Domain;
using Inventory.Api.Domain.Messaging;
using Inventory.Api.Infrastructure.Messaging;

namespace Inventory.Api.Features.AddInventory;

public interface IAddInventoryService
{
    Task<OperationResult> ExecuteAsync(AddInventoryRequest request);
}

public class AddInventoryService : IAddInventoryService
{
    private readonly MessageConfig _messageConfig;
    private readonly IMessagePublisher _messagePublisher;

    public AddInventoryService(MessageConfig messageConfig, IMessagePublisher messagePublisher)
    {
        _messageConfig = messageConfig;
        _messagePublisher = messagePublisher;
    }

    public async Task<OperationResult> ExecuteAsync(AddInventoryRequest request) =>
        (await _messagePublisher.PublishAsync(_messageConfig.QueueName, new PublishableMessage<AddInventoryRequest>(request)).Run())
        .Match(
            _ => OperationResult.Success(),
            OperationResult.Failure
        );
}
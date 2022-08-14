using Azure.Storage.Queues;
using Inventory.Api.Domain.DataTransfer;
using Inventory.Api.Domain.Messaging;
using Inventory.Api.Infrastructure.Extensions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Inventory.Api.Infrastructure.Messaging;

public sealed class MessagePublisher : IMessagePublisher
{
    private readonly IJsonService _jsonService;
    private readonly QueueServiceClient _queueServiceClient;

    public MessagePublisher(IJsonService jsonService, QueueServiceClient queueServiceClient)
    {
        _jsonService = jsonService;
        _queueServiceClient = queueServiceClient;
    }

    public Aff<Unit> PublishAsync<T>(string queue, IMessage<T> message) where T: class
    {
        var messagePublishEffect = (from queueClient in _queueServiceClient.GetOrCreateQueueClientAsync(queue)
                from publishReceipt in queueClient.PublishMessageAsync(message, () => _jsonService)
                select publishReceipt)
            .BiMap(
                _ => unit,
                error => error
            );

        return messagePublishEffect;
    }
}
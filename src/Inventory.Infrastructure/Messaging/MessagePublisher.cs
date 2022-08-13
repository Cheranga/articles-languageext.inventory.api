using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Inventory.Domain;
using Inventory.Domain.DataTransfer;
using Inventory.Domain.Messaging;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Inventory.Infrastructure.Messaging;

public sealed class MessagePublisher : IMessagePublisher
{
    private readonly MessageConfig _config;
    private readonly IJsonService _jsonService;
    private readonly QueueServiceClient _queueServiceClient;

    public MessagePublisher(MessageConfig config, IJsonService jsonService, QueueServiceClient queueServiceClient)
    {
        _config = config;
        _jsonService = jsonService;
        _queueServiceClient = queueServiceClient;
    }


    public Aff<Unit> PublishAsync<T>(T message) where T : class, IMessage
    {
        var messagePublishEffect = (from queueClient in GetQueueClient(_config.QueueName)
                from _ in CreateIfNotExistsAsync(queueClient)
                from serializedContent in SerializeMessageDataAsync(message)
                from publishReceipt in PublishMessageAsync(queueClient, serializedContent)
                select publishReceipt)
            .BiMap(
                _ => unit,
                error => error
            );

        return messagePublishEffect;
    }

    private Aff<QueueClient> GetQueueClient(string queueName) =>
        EffMaybe<QueueClient>(() => _queueServiceClient.GetQueueClient(queueName))
            .ToAff().MapFail(error => Error.New(ErrorCodes.ErrorWhenGettingQueueClient, ErrorMessages.ErrorWhenGettingQueueClient, error.ToException()));

    private Aff<Response> CreateIfNotExistsAsync(QueueClient client) =>
        AffMaybe<Response>(async () => await client.CreateIfNotExistsAsync())
            .MapFail(error => Error.New(ErrorCodes.ErrorWhenCreatingQueue, ErrorMessages.ErrorWhenCreatingQueue, error.ToException()));

    private Aff<string> SerializeMessageDataAsync<TData>(TData data) where TData : class =>
        _jsonService.SerializeAsync(data).MapFail(error => Error.New(ErrorCodes.ErrorWhenSerializing, ErrorMessages.ErrorWhenSerializing, error.ToException()));

    private Aff<Response<SendReceipt>> PublishMessageAsync(QueueClient client, string content) =>
        AffMaybe<Response<SendReceipt>>(async () => await client.SendMessageAsync(content))
            .MapFail(error => Error.New(ErrorCodes.ErrorWhenPublishingMessage, ErrorMessages.ErrorWhenPublishingMessage, error.ToException()));

}
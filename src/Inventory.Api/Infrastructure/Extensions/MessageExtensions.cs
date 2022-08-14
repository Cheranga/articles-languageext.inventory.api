using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Inventory.Api.Domain;
using Inventory.Api.Domain.DataTransfer;
using Inventory.Api.Domain.Messaging;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Inventory.Api.Infrastructure.Extensions;

internal static class MessageExtensions
{
    /// <summary>
    /// Gets or creates a `QueueClient` instance.
    /// </summary>
    /// <param name="serviceClient">The `QueueServiceClient` instance which will be used to create the `QueueClient`.</param>
    /// <param name="queueName">The name of the queue.</param>
    /// <returns>An asynchronous effect which wraps up the `QueueClient` instance.</returns>
    public static Aff<QueueClient> GetOrCreateQueueClientAsync(this QueueServiceClient serviceClient, string queueName) =>
        (from queueClient in EffMaybe<QueueClient>(() => serviceClient.GetQueueClient(queueName))
            from _ in AffMaybe<bool>(async () =>
            {
                var response = await queueClient.CreateIfNotExistsAsync();
                return response == null;
            })
            select queueClient)
        .MapFail(error => Error.New(ErrorCodes.ErrorWhenGettingQueueClient, ErrorMessages.ErrorWhenGettingQueueClient, error.ToException()));

    /// <summary>
    /// Publishes the message to the queue.
    /// </summary>
    /// <param name="client">The `QueueClient` which will be used to publish the message or event.</param>
    /// <param name="message">The message data to be published into the queue.</param>
    /// <param name="dataTransferService">The data transfer service, which will be used </param>
    /// <returns></returns>
    public static Aff<Response<SendReceipt>> PublishMessageAsync<T>(this QueueClient client, IMessage<T> message, Func<IDataTransferService> dataTransferService) where T:class =>
        (from serializedContent in dataTransferService().SerializeAsync(message.Data)
            from receipt in AffMaybe<Response<SendReceipt>>(async () => await client.SendMessageAsync(serializedContent))
            select receipt)
        .MapFail(error => Error.New(ErrorCodes.ErrorWhenPublishingMessage, ErrorMessages.ErrorWhenPublishingMessage, error.ToException()));
}
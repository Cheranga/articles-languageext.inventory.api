using LanguageExt;

namespace Inventory.Api.Domain.Messaging;

/// <summary>
/// Abstraction of a message or an even publication.
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes an `IMessage` object to the underlying message broker.
    /// </summary>
    /// <param name="queue">The queue name where the message/event will be published in.</param>
    /// <param name="message">The message to publish.</param>
    /// <typeparam name="T">The type of the message.</typeparam>
    /// <returns>An asynchronous effect which wraps the operation status.</returns>
    Aff<Unit> PublishAsync<T>(string queue, IMessage<T> message) where T : class;
}
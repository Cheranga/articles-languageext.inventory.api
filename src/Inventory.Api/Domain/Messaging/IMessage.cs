namespace Inventory.Api.Domain.Messaging;

/// <summary>
/// Marker interface to represent a model which can be published as a message or an event.
/// </summary>
public interface IMessage<T> where T:class
{
    public T Data { get; }
}
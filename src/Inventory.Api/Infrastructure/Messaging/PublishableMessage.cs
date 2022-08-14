using Inventory.Api.Domain.Messaging;

namespace Inventory.Api.Infrastructure.Messaging;

public class PublishableMessage<T> : IMessage<T> where T : class
{
    public T Data { get; }

    public PublishableMessage(T data)
    {
        Data = data;
    }
}
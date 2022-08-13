using LanguageExt;

namespace Inventory.Domain.Messaging;

public interface IMessagePublisher
{
    Aff<Unit> PublishAsync<T>(T message) where T : class, IMessage;
}
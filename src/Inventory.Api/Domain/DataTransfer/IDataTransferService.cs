using LanguageExt;

namespace Inventory.Api.Domain.DataTransfer;

/// <summary>
/// A generic interface to represent data transfer operations.
/// </summary>
public interface IDataTransferService
{
    /// <summary>
    /// Given a string, will deserialize into the target `TData` type.
    /// </summary>
    /// <param name="content">The content, must be non null or empty.</param>
    /// <typeparam name="TData">The target type.</typeparam>
    /// <returns>An asynchronous effect, which wraps up the deserialized model from the content.</returns>
    Aff<TData> DeserializeAsync<TData>(string content) where TData : class;
    
    /// <summary>
    /// Given an object, will serialize into a string.
    /// </summary>
    /// <param name="data">The data to serialize to a string representation.</param>
    /// <typeparam name="TData">The data to serialize.</typeparam>
    /// <returns>An asynchronous effect, which wraps up the serialized string.</returns>
    Aff<string> SerializeAsync<TData>(TData data) where TData : class;
}
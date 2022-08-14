using System.Text.Json;
using LanguageExt;

namespace Inventory.Api.Domain.DataTransfer;

/// <summary>
/// A service to serialize classes to strings or deserialize into an object given a string
/// </summary>
public interface IJsonService : IDataTransferService
{
    /// <summary>
    /// Given a string, will deserialize into the target `TData` type.
    /// </summary>
    /// <param name="content">The content, must be non null or empty.</param>
    /// <param name="options">How the deserialization should happen.</param>
    /// <typeparam name="TData">The target type.</typeparam>
    /// <returns>An asynchronous effect, which wraps up the deserialized model from the content.</returns>
    Aff<TData> DeserializeAsync<TData>(string content, Func<JsonSerializerOptions> options) where TData : class;
    
    /// <summary>
    /// Given an object, will serialize into a string.
    /// </summary>
    /// <param name="data">The data to serialize to a string representation.</param>
    /// <param name="options">How the serialization should happen.</param>
    /// <typeparam name="TData">The data to serialize.</typeparam>
    /// <returns>An asynchronous effect, which wraps up the serialized string.</returns>
    Aff<string> SerializeAsync<TData>(TData data, Func<JsonSerializerOptions> options) where TData : class;
}
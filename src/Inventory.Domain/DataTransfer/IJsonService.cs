using System.Text.Json;
using LanguageExt;

namespace Inventory.Domain.DataTransfer;

public interface IJsonService
{
    Aff<TData> DeserializeAsync<TData>(string content, Func<JsonSerializerOptions>? options=null) where TData : class;
    Aff<string> SerializeAsync<TData>(TData data, Func<JsonSerializerOptions>? options = null) where TData : class;
}
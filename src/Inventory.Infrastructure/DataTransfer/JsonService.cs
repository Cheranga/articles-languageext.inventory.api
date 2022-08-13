using System.Text.Json;
using System.Text.Json.Serialization;
using Inventory.Domain;
using Inventory.Domain.DataTransfer;
using Inventory.Infrastructure.Extensions;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Inventory.Infrastructure.DataTransfer;

public sealed class JsonService : IJsonService
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = false,
        NumberHandling = JsonNumberHandling.Strict,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode
    };

    public Aff<TData> DeserializeAsync<TData>(string content, Func<JsonSerializerOptions>? options = null) where TData : class
    {
        var getDeserializedModelEffect = use(content.ToMemoryStream(), stream =>
            from model in stream.Deserialize<TData>(options == null ? DefaultOptions : options())
            select model);

        return getDeserializedModelEffect;
    }

    public Aff<string> SerializeAsync<TData>(TData data, Func<JsonSerializerOptions>? options = null) where TData : class
    {
        if (data == null)
        {
            return FailAff<string>(Error.New(ErrorCodes.InvalidData, ErrorMessages.InvalidData));
        }
        
        var getSerializedStringEffect = use(StreamExtensions.ToMemoryStream(), stream =>
            from _ in stream.Serialize(data, options == null ? DefaultOptions : options())
            from reader in stream.GetStreamReader()
            from content in reader.ReadString()
            select content);

        return getSerializedStringEffect;
    }

    

    

    

    

    

    
}
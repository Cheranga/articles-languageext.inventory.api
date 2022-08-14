using System.Text.Json;
using System.Text.Json.Serialization;
using Inventory.Api.Domain;
using Inventory.Api.Domain.DataTransfer;
using Inventory.Api.Infrastructure.Extensions;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Inventory.Api.Infrastructure.DataTransfer;

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

    public Aff<TData> DeserializeAsync<TData>(string content, Func<JsonSerializerOptions> options) where TData : class =>
        from validatedContent in Optional(content).ToAff(Error.New(ErrorCodes.InvalidData, ErrorMessages.InvalidData))
        from model in validatedContent.DeserializeAsync<TData>(options)
        select model;

    public Aff<string> SerializeAsync<TData>(TData data, Func<JsonSerializerOptions> options) where TData : class =>
        from model in Optional(data).ToAff(Error.New(ErrorCodes.InvalidData, ErrorMessages.InvalidData))
        from content in model.SerializeAsync(options)
        select content;

    public Aff<TData> DeserializeAsync<TData>(string content) where TData : class =>
        DeserializeAsync<TData>(content, () => DefaultOptions);

    public Aff<string> SerializeAsync<TData>(TData data) where TData : class => SerializeAsync(data, () =>
        DefaultOptions);
}
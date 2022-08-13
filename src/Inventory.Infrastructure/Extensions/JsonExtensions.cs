using System.Text.Json;
using Inventory.Domain;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Inventory.Infrastructure.Extensions;

internal static class JsonExtensions
{
    public static Aff<Stream> Serialize<TData>(this Stream stream, TData data, JsonSerializerOptions options) =>
        // TODO: find if this can be done using an `AffMaybe` 
        TryAsync(async () =>
        {
            await JsonSerializer.SerializeAsync(stream, data, options);
            return stream;
        }).ToAff();


    public static Aff<TData> Deserialize<TData>(this Stream stream, JsonSerializerOptions options) =>
        AffMaybe<TData>(async () => (await JsonSerializer.DeserializeAsync<TData>(stream, options))!)
            .MapFail(error => Error.New(ErrorCodes.ErrorWhenDeserializing, ErrorMessages.ErrorWhenDeserializing, error.ToException()));
}
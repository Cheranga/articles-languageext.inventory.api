using System.Text;
using System.Text.Json;
using Inventory.Api.Domain;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Inventory.Api.Infrastructure.Extensions;

internal static class JsonExtensions
{
    private static Aff<Unit> JsonSerializeAsync<TData>(this Stream stream, TData data, Func<JsonSerializerOptions> options) =>
        AffMaybe<Unit>(async () =>
        {
            await JsonSerializer.SerializeAsync(stream, data, options());
            return unit;
        });

    private static Aff<string> ReadContentAsync(this Stream stream) =>
        from content in use(Eff(() => new StreamReader(stream)), reader =>
            from c in AffMaybe<string>(async () =>
            {
                stream.Position = 0;
                var streamContent = await reader.ReadToEndAsync();
                return streamContent;
            })
            select c)
        select content;

    /// <summary>
    /// Serializes the model to JSON.
    /// </summary>
    /// <param name="data">The object to serialize into JSON.</param>
    /// <param name="options">JSON serialization options.</param>
    /// <typeparam name="TData">Type of the model.</typeparam>
    /// <returns>An asynchronous effect which wraps up the JSON serialized string.</returns>
    public static Aff<string> SerializeAsync<TData>(this TData data, Func<JsonSerializerOptions> options) where TData : class =>
        use(Eff(() => new MemoryStream()), stream =>
                from _ in stream.JsonSerializeAsync(data, options)
                from content in stream.ReadContentAsync()
                select content)
            .MapFail(error => Error.New(ErrorCodes.ErrorWhenSerializing, ErrorMessages.ErrorWhenSerializing, error.ToException()));

    /// <summary>
    /// Deserializes the JSON string to the target model.
    /// </summary>
    /// <param name="content">The JSON string content.</param>
    /// <param name="options">The options to deserialize from the JSON string.</param>
    /// <typeparam name="TData">The target type.</typeparam>
    /// <returns>An asynchronous effect which wraps up the target model.</returns>
    public static Aff<TData> DeserializeAsync<TData>(this string content, Func<JsonSerializerOptions> options) where TData : class =>
        use(Eff(() => new MemoryStream(Encoding.UTF8.GetBytes(content))), stream =>
                from model in AffMaybe<TData>(async () => (await JsonSerializer.DeserializeAsync<TData>(stream, options()))!)
                select model)
            .MapFail(error => Error.New(ErrorCodes.ErrorWhenDeserializing, ErrorMessages.ErrorWhenDeserializing, error.ToException()));
}
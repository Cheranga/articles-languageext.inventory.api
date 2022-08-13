using System.Text;
using Inventory.Domain;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Inventory.Infrastructure.Extensions;

internal static class StreamExtensions
{
    public static Eff<Stream> ToMemoryStream() =>
        Eff<Stream>(() => new MemoryStream());

    public static Eff<Stream> ToMemoryStream(this string content) =>
        Eff<Stream>(() => new MemoryStream(Encoding.UTF8.GetBytes(content)));

    public static Aff<StreamReader> GetStreamReader(this Stream stream) =>
        EffMaybe<StreamReader>(() =>
            {
                var reader = new StreamReader(stream);
                stream.Position = 0;
                return reader;
            })
            .MapFail(error => Error.New(ErrorCodes.CannotCreateStreamReader, ErrorMessages.CannotCreateStreamReader, error.ToException()));
    
    public static Aff<string> ReadString(this StreamReader reader) =>
        AffMaybe<string>(async () => await reader.ReadToEndAsync())
            .MapFail(error => Error.New(ErrorCodes.ErrorWhenReadingStream, ErrorMessages.ErrorWhenReadingStream, error.ToException()));
}
namespace Inventory.Domain;

public static class ErrorCodes
{
    public const int ErrorWhenCreatingQueue = 500;
    public const int ErrorWhenPublishingMessage = 501;
    public const int ErrorWhenGettingQueueClient = 502;

    public const int ErrorWhenReadingStream = 503;
    public const int ErrorWhenDeserializing = 504;
    public const int CannotCreateStreamReader = 505;
    public const int InvalidData = 506;
    public const int ErrorWhenSerializing = 507;
}

public static class ErrorMessages
{
    public const string ErrorWhenCreatingQueue = "error occurred when creating the queue";
    public const string ErrorWhenPublishingMessage = "error occurred when publishing message to the queue";
    public const string ErrorWhenGettingQueueClient = "error occurred when getting a queue client through queue service client";
    public const string ErrorWhenReadingStream = "error occurred when reading stream";
    public const string ErrorWhenDeserializing = "error occurred when deserializing content";
    public const string CannotCreateStreamReader = "error occurred when creating a stream reader";
    public const string InvalidData = "invalid data";
    public const string ErrorWhenSerializing = "error occurred when deserializing data";
}
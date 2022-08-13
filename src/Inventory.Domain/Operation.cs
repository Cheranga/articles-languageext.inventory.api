namespace Inventory.Domain;

public class Operation
{
    public string ErrorCode { get; init; }
    public string ErrorMessage { get; init; }
    
    private static Operation Success()
    {
        return new Operation();
    }

    private static Operation Failure(string errorCode, string errorMessage)
    {
        return new Operation
        {
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        };
    }
}


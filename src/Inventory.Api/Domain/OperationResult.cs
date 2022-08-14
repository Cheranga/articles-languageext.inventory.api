using FluentValidation.Results;
using LanguageExt.Common;

namespace Inventory.Api.Domain;

public class OperationResult
{
    public ValidationResult ValidationResult { get; init; }
    public bool IsSuccessful => ValidationResult == null || ValidationResult.IsValid;

    public static OperationResult Success() =>
        new OperationResult();

    public static OperationResult Failure(Error error) =>
        new OperationResult
        {
            ValidationResult = new ValidationResult(new[] {new ValidationFailure(error.Code.ToString(), error.Message)})
        };
}
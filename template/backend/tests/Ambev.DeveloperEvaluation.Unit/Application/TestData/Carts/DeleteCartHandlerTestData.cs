using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

/// <summary>
/// Provides methods for generating test data for DeleteCartHandler unit tests.
/// </summary>
public static class DeleteCartHandlerTestData
{
    public static DeleteCartCommand GenerateValidCommand() =>
        new DeleteCartCommand(Guid.NewGuid());

    public static DeleteCartCommand GenerateInvalidCommand() =>
        new DeleteCartCommand(Guid.Empty);
}
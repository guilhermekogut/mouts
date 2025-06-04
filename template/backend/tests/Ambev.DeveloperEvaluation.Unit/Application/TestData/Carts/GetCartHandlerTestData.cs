using Ambev.DeveloperEvaluation.Application.Carts.GetCart;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

/// <summary>
/// Provides methods for generating test data for GetCartHandler unit tests.
/// </summary>
public static class GetCartHandlerTestData
{
    public static GetCartCommand GenerateValidCommand() =>
        new GetCartCommand(Guid.NewGuid());

    public static GetCartCommand GenerateInvalidCommand() =>
        new GetCartCommand(Guid.Empty);
}
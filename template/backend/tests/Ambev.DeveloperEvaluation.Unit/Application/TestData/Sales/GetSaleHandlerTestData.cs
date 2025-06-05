using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

/// <summary>
/// Provides methods for generating test data for GetSaleHandler unit tests.
/// </summary>
public static class GetSaleHandlerTestData
{
    public static GetSaleCommand GenerateValidCommand() =>
        new GetSaleCommand(Guid.NewGuid());

    public static GetSaleCommand GenerateInvalidCommand() =>
        new GetSaleCommand(Guid.Empty);
}
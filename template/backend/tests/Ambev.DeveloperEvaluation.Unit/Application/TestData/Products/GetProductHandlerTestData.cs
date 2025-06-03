using Ambev.DeveloperEvaluation.Application.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

/// <summary>
/// Provides methods for generating test data for GetProductHandler unit tests.
/// </summary>
public static class GetProductHandlerTestData
{
    public static GetProductCommand GenerateValidCommand() =>
        new GetProductCommand(Guid.NewGuid());

    public static GetProductCommand GenerateInvalidCommand() =>
        new GetProductCommand(Guid.Empty);
}
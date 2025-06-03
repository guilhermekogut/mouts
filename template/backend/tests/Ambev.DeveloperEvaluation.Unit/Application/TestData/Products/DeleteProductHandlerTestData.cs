using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

/// <summary>
/// Provides methods for generating test data for DeleteProductHandler unit tests.
/// </summary>
public static class DeleteProductHandlerTestData
{
    public static DeleteProductCommand GenerateValidCommand() =>
        new DeleteProductCommand(Guid.NewGuid());

    public static DeleteProductCommand GenerateInvalidCommand() =>
        new DeleteProductCommand(Guid.Empty);
}
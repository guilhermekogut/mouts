namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Result returned after deleting a product.
/// </summary>
public class DeleteProductResult
{
    public Guid Id { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}
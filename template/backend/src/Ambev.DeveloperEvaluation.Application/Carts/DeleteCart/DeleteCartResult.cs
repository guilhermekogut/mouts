namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Result returned after deleting a cart.
/// </summary>
public class DeleteCartResult
{
    public Guid Id { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}
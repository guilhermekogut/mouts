namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public class ListCartsResult
{
    public IEnumerable<ListCartItemResult> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
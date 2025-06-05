namespace Ambev.DeveloperEvaluation.Domain.Repositories.Results;

public class ProductExistenceItem
{
    public Guid ProductId { get; }
    public bool Exists { get; }

    public ProductExistenceItem(Guid productId, bool exists)
    {
        ProductId = productId;
        Exists = exists;
    }
}

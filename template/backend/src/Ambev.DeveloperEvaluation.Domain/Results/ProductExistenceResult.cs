namespace Ambev.DeveloperEvaluation.Domain.Repositories.Results;
public class ProductExistenceResult
{
    public IReadOnlyCollection<ProductExistenceItem> Items { get; }
    public bool AllExist => Items.All(x => x.Exists);

    public ProductExistenceResult(IEnumerable<ProductExistenceItem> items)
    {
        Items = items.ToList().AsReadOnly();
    }
}


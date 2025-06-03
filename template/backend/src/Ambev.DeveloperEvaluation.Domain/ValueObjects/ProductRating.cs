namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Value object for product rating.
/// </summary>
public class ProductRating
{
    /// <summary>
    /// Gets or sets the product's average rating.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the number of ratings received.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRating"/> class.
    /// </summary>
    public ProductRating() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRating"/> class with the specified rate and count.
    /// </summary>
    /// <param name="rate">The product's average rating.</param>
    /// <param name="count">The number of ratings received.</param>
    public ProductRating(decimal rate, int count)
    {
        Rate = rate;
        Count = count;
    }

    /// <summary>
    /// Validates the product rating values.
    /// Ensures that the rate is between 0 and 5, and the count is zero or positive.
    /// </summary>
    public void Validate()
    {
        if (Rate < 0m || Rate > 5m)
            throw new ArgumentOutOfRangeException(nameof(Rate), "Rate must be between 0 and 5.");
        if (Count < 0)
            throw new ArgumentOutOfRangeException(nameof(Count), "Count must be zero or positive.");
    }
}
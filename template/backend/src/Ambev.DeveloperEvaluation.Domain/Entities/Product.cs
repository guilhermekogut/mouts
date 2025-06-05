using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product in the system.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets the product title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product image URL.
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product rating.
    /// </summary>
    public ProductRating Rating { get; set; } = new();

    /// <summary>
    /// Gets or sets the creation date and time of the product.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date and time of the product.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title) || Title.Length > 200)
            throw new ArgumentException("Title is required and must be at most 200 characters.");
        if (Price < 0)
            throw new ArgumentException("Price must be zero or positive.");
        if (string.IsNullOrWhiteSpace(Description) || Description.Length > 1000)
            throw new ArgumentException("Description is required and must be at most 1000 characters.");
        if (string.IsNullOrWhiteSpace(Category) || Category.Length > 100)
            throw new ArgumentException("Category is required and must be at most 100 characters.");
        if (string.IsNullOrWhiteSpace(Image))
            throw new ArgumentException("Image is required.");

        // Validate the value object
        Rating?.Validate();
    }
}
namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public class ListProductsRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Order { get; set; }
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        public decimal? RatingRate { get; set; }
        public decimal? MinRatingRate { get; set; }
        public decimal? MaxRatingRate { get; set; }
        public int? RatingCount { get; set; }
        public int? MinRatingCount { get; set; }
        public int? MaxRatingCount { get; set; }
    }
}
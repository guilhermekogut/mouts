using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public class ListProductsResponse
    {
        public IEnumerable<ListProductsItemResponse> Data { get; set; } = [];
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
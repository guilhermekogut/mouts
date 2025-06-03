using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class ProductItemResultProfile : Profile
{
    public ProductItemResultProfile()
    {
        CreateMap<ProductItemResult, ListProductsItemResponse>();
    }
}

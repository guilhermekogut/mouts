using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class ProductRatingResultProfile : Profile
{
    public ProductRatingResultProfile()
    {
        CreateMap<ProductRatingResult, ProductRatingResponse>();
    }
}

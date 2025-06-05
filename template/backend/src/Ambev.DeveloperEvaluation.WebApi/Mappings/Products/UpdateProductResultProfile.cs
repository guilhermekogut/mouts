using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class UpdateProductResultProfile : Profile
{
    public UpdateProductResultProfile()
    {
        CreateMap<UpdateProductResult, UpdateProductResponse>();
    }
}
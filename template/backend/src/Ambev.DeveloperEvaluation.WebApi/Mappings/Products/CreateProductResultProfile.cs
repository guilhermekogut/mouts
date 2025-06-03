using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class CreateProductResultProfile : Profile
{
    public CreateProductResultProfile()
    {
        CreateMap<CreateProductResult, CreateProductResponse>();
    }
}

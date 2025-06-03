using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Profile for mapping between Product entity and UpdateProductResult.
/// </summary>
public class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductCommand, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Rating, opt => opt.Ignore());

        CreateMap<Product, UpdateProductResult>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingResult
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}
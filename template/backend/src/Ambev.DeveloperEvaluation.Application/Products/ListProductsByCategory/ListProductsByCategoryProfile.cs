using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProductsByCategory;

public class ListProductsByCategoryProfile : Profile
{
    public ListProductsByCategoryProfile()
    {
        CreateMap<Product, ProductItemResult>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingResult
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}
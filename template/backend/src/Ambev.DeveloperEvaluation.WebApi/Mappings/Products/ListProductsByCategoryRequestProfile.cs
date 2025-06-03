using Ambev.DeveloperEvaluation.Application.Products.ListProductsByCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProductsByCategory;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class ListProductsByCategoryRequestProfile : Profile
{
    public ListProductsByCategoryRequestProfile()
    {
        CreateMap<ListProductsByCategoryRequest, ListProductsByCategoryCommand>()
             .ForMember(dest => dest.Category, opt => opt.Ignore());
    }
}
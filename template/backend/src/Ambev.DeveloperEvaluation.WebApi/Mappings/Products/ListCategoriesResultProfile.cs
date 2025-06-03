using Ambev.DeveloperEvaluation.Application.Products.ListCategories;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListCategories;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Products;

public class ListCategoriesResultProfile : Profile
{
    public ListCategoriesResultProfile()
    {
        CreateMap<ListCategoriesResult, ListCategoriesResponse>();
    }
}
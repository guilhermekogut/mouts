using Ambev.DeveloperEvaluation.Application.Products.ListCategories;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListCategories
{
    public class ListCategoriesProfile : Profile
    {
        public ListCategoriesProfile()
        {
            CreateMap<ListCategoriesRequest, ListCategoriesCommand>();
            CreateMap<ListCategoriesResult, ListCategoriesResponse>();
        }
    }
}
using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Users;

public class ListUsersResultProfile : Profile
{
    public ListUsersResultProfile()
    {
        CreateMap<ListUsersResult, ListUsersResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
    }
}
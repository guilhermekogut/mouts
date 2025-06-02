using Ambev.DeveloperEvaluation.Application.Users.ListUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class ListUsersRequestProfile : Profile
{
    public ListUsersRequestProfile()
    {
        CreateMap<ListUsersRequest, ListUsersQueryCommand>();
    }
}
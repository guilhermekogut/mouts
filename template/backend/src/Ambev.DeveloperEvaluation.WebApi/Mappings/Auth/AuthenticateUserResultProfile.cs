using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Auth;

public class AuthenticateUserResultProfile : Profile
{
    public AuthenticateUserResultProfile()
    {
        CreateMap<AuthenticateUserResult, AuthenticateUserResponse>();
    }
}
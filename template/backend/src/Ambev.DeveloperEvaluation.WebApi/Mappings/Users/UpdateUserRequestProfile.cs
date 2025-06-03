using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Users;

public class UpdateUserRequestProfile : Profile
{
    public UpdateUserRequestProfile()
    {
        CreateMap<UpdateUserRequest, UpdateUserCommand>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new NameResult(src.Name.Firstname, src.Name.Lastname)))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new AddressResult(
                src.Address.City,
                src.Address.Street,
                src.Address.Number,
                src.Address.Zipcode,
                new GeolocationResult(src.Address.Geolocation.Lat, src.Address.Geolocation.Long)
            )));
    }
}
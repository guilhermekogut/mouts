using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Profile for mapping between User entity and GetUserResponse
/// </summary>
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser operation
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<User, GetUserResult>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => new NameResult(src.Name.Firstname, src.Name.Lastname)))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new AddressResult(
                src.Address.City,
                src.Address.Street,
                src.Address.Number,
                src.Address.Zipcode,
                new GeolocationResult(src.Address.Geolocation.Lat, src.Address.Geolocation.Long)
            )));
    }
}


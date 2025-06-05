using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUsers;

/// <summary>
/// Profile for mapping between User entity and GetUserResult
/// </summary>
public class ListUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListUsers operation
    /// </summary>
    public ListUsersProfile()
    {
        CreateMap<User, ListUserItemResult>()
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


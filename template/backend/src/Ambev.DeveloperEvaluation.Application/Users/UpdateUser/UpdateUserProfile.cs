using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Profile for mapping between User entity and UpdateUserResponse
/// </summary>
public class UpdateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateUser operation
    /// </summary>
    public UpdateUserProfile()
    {
        CreateMap<UpdateUserCommand, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name(src.Name.Firstname, src.Name.Lastname)))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address(
                src.Address.City,
                src.Address.Street,
                src.Address.Number,
                src.Address.Zipcode,
                new Geolocation(src.Address.Geolocation.Lat, src.Address.Geolocation.Long)
            )));

        CreateMap<User, UpdateUserResult>()
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

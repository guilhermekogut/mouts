﻿using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.Users;

public class GetUserResultProfile : Profile
{
    public GetUserResultProfile()
    {
        CreateMap<GetUserResult, GetUserResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new NameResponse
            {
                Firstname = src.Name.Firstname,
                Lastname = src.Name.Lastname
            }))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new AddressResponse
            {
                City = src.Address.City,
                Street = src.Address.Street,
                Number = src.Address.Number,
                Zipcode = src.Address.Zipcode,
                Geolocation = new GeolocationResponse
                {
                    Lat = src.Address.Geolocation.Lat,
                    Long = src.Address.Geolocation.Long
                }
            }));
    }
}
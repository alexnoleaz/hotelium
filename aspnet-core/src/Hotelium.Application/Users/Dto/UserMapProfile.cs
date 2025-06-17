using AutoMapper;

using Hotelium.Users.Values;

namespace Hotelium.Users.Dto;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<AddressDto, Address>()
            .ConvertUsing(x =>
                x == null || (string.IsNullOrWhiteSpace(x.Street) && string.IsNullOrWhiteSpace(x.City))
                ? default! : new Address(x.Street!, x.City!)
        );

        CreateMap<CreateUserDto, User>()
            .ForMember(x => x.Roles, opt => opt.Ignore());

        CreateMap<UpdateUserDto, User>()
            .ForMember(x => x.Roles, opt => opt.Ignore());

        CreateMap<User, UserDto>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => $"{x.Name} {x.Surname}"));
    }
}
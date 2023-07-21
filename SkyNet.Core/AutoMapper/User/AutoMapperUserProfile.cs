using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SkyNet.Core.DTOs.User;
using SkyNet.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.AutoMapper.User
{
    public class AutoMapperUserProfile : Profile
    {
        public AutoMapperUserProfile()
        {
            CreateMap<UsersDTO, AppUser>().ReverseMap();
            CreateMap<UpdateUserDTO, AppUser>().ReverseMap();
            CreateMap<CreateUserDTO, AppUser>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
        }
    }
}

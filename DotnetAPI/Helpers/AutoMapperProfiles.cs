using AutoMapper;
using DotnetAPI.Dto;
using DotnetAPI.Model;

namespace DotnetAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, AppUser>();
        }
    }
}
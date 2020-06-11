using System.Collections.Generic;
using AutoMapper;
using DotnetAPI.Dto;
using DotnetAPI.Model;

namespace DotnetAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, AppUser>();
            CreateMap<UserForEditDto, AppUser>();
            CreateMap<ClassForCrudDto, Class>();
            CreateMap<Class, ClassForListDto>();
            CreateMap<AppUser, UserForListDto>();
            CreateMap<PublicationAddDto, Publication>();
            CreateMap<Publication, PublicationForListDto>();
            CreateMap<CommentForAddDto, Comment>();
            CreateMap<Comment, CommentForListDto>();
            CreateMap<Attachment, AttachmentForListDto>();
        }
    }
}
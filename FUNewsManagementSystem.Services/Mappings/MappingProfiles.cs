using AutoMapper;
using FUNewsManagementSystem.Repositories.Entity;
using FUNewsManagementSystem.Services.Models.AccoutDTOs;
using FUNewsManagementSystem.Services.Models.ArticleDTOs;
using FUNewsManagementSystem.Services.Models.CategoriesDTOs;
using FUNewsManagementSystem.Services.Models.LoginDTOs;
using FUNewsManagementSystem.Services.Models.TagDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FUNewsManagementSystem.Services.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SystemAccount, LoginResponseDto>().ReverseMap();
            CreateMap<RegisterRequestDto, SystemAccount>().ReverseMap();

            CreateMap<NewsArticle, NewsArticleDto>().ReverseMap();
            CreateMap<CreateNewsArticleDto, NewsArticle>().ReverseMap();
            CreateMap<UpdateNewsArticleDto, NewsArticle>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();

            CreateMap<SystemAccount, SystemAccountDto>().ReverseMap();
            CreateMap<CreateSystemAccountDto, SystemAccount>().ReverseMap();
            CreateMap<UpdateSystemAccountDto, SystemAccount>().ReverseMap();

            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<CreateTagDto, Tag>().ReverseMap();
            CreateMap<UpdateTagDto, Tag>().ReverseMap();
        }
    }
}

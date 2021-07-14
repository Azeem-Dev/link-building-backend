
using AutoMapper;
using link_building.Dtos.Category;
using link_building.Dtos.Link;
using link_building.Dtos.SubCategory;
using link_building.Models.Category;
using link_building.Models.Link;
using link_building.Models.SubCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace link_building
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryResponseDto, CategoryEntity>().ReverseMap();
            CreateMap<LinkEntity, LinkResponseDto>().ForMember(c=>c.Categories,c=>c.MapFrom(c=>c.LinkCategories.Select(c=>c.Category))).ReverseMap();
            CreateMap<SubCategoryEntityDto, SubCategoryEntity>().ReverseMap();
            CreateMap<CategoryEntityDto, CategoryEntity>().ReverseMap();
            CreateMap<LinkCreateSubCategoryResponseDto, SubCategoryEntity>().ReverseMap();
        }
    }
}

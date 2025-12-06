using App.Business.DTOs.Categories;
using App.Core.Entities;
using AutoMapper;

namespace App.Business.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
        }
    }
}

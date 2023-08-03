using AutoMapper;
using ECommerce.Models.ViewModels;
using ECommerce.Models;

namespace ECommerce.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<AddCategoryViewModel, Category>()
                .ForMember(viewModel => viewModel.Name, opt => opt.MapFrom(viewModel => viewModel.Name))
                .ReverseMap();
        }
    }
}

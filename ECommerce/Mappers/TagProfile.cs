using AutoMapper;
using ECommerce.Models;
using ECommerce.Models.ViewModels;

namespace ECommerce.Mappers
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<AddTagViewModel, Tag>()
               .ForMember(viewModel => viewModel.Name, opt => opt.MapFrom(viewModel => viewModel.Name))
               .ReverseMap();
        }
    }
}

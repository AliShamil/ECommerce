using AutoMapper;
using ECommerce.Models;
using ECommerce.Models.ViewModels;

namespace ECommerce.Mappers
{
    public class UserProductProfile : Profile
    {
        public UserProductProfile()
        {
            CreateMap<Product, UserProductViewModel>()
                .ForMember(viewModel => viewModel.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(viewModel => viewModel.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(viewModel => viewModel.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(viewModel => viewModel.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(viewModel => viewModel.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .AfterMap((p, model) =>
                {
                    var names = new List<string>();

                    foreach (var pt in p.ProductTags)
                    {
                        names.Add(pt.Tag.Name);
                    }

                    model.TagNames = names.ToArray();
                });
        }
    }
}

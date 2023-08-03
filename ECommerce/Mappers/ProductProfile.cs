using ECommerce.Models;
using AutoMapper;
using ECommerce.Helpers;
using ECommerce.Models.ViewModels;

namespace ECommerce.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewModel, Product>()
               .ForMember(viewModel => viewModel.Name, opt => opt.MapFrom(viewModel => viewModel.Name))
               .ForMember(viewModel => viewModel.Description, opt => opt.MapFrom(viewModel => viewModel.Description))
               .ForMember(viewModel => viewModel.CategoryId, opt => opt.MapFrom(viewModel => viewModel.CategoryId))
               .AfterMap(async (viewModel, product) =>
               {
                   string path = await UploadFileHelper.UploadFile(viewModel.ImageUrl);
                   product.ImageUrl = path;
               });
        }
    }
}

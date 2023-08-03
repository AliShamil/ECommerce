using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ViewModels
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Please enter the name.")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the description.")]
        [StringLength(255, MinimumLength = 10)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please choose the image.")]
        public IFormFile ImageUrl { get; set; }
        [Required(ErrorMessage = "Please choose the Category.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please choose at least one tag.")]
        public int[] TagIds { get; set; }

        public static implicit operator AddProductViewModel(Product p)
        {
            AddProductViewModel model = new AddProductViewModel();
            model.Name = p.Name;
            model.Description = p.Description;
            model.CategoryId = p.CategoryId;

            var stream = new FileStream($@"wwwroot\{p.ImageUrl}", FileMode.Open, FileAccess.Read);
            model.ImageUrl = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(stream.Name));

            var list = p.ProductTags.ToList();
            var temp = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                temp.Add(list[i].TagId);
            }

            model.TagIds= temp.ToArray();

            return model;
        }
    }
}

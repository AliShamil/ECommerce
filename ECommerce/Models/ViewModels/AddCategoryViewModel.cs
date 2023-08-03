using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ViewModels
{
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = "Please enter a name.")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        public static implicit operator AddCategoryViewModel(Category c)
        {
            AddCategoryViewModel model = new();
            model.Name = c.Name;
            return model;
        }
    }
}

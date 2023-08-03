namespace ECommerce.Models.ViewModels
{
    public class UserProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public string[] TagNames { get; set; }
    }
}

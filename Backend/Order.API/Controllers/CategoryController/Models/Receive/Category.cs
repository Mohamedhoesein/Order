namespace Order.API.Controllers.CategoryController.Models.Receive
{
    public class Category
    {
        public string Name { get; set; }
        public string[] Subcategories { get; set; }
    }
}
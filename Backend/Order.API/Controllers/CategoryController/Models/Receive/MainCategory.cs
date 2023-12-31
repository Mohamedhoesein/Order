namespace Order.API.Controllers.CategoryController.Models.Receive
{
    public class MainCategory
    {
        public string Name { get; set; }
        public Category[] Categories { get; set; }
    }
}
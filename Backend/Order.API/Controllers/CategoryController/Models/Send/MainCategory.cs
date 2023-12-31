namespace Order.API.Controllers.CategoryController.Models.Send
{
    public class MainCategory
    {
        public bool Deleted { get; set; }
        public string Name { get; set; }
        public Category[] Categories { get; set; }

        public MainCategory(){}

        public MainCategory(Context.MainCategory mainCategory)
        {
            Deleted = mainCategory.Deleted;
            Name = mainCategory.Name;
            Categories = mainCategory.Categories.Select(category => new Category(category))
                .OrderBy(category => category.Name).ToArray();
        }
    }
}
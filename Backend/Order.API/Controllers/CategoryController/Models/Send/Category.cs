namespace Order.API.Controllers.CategoryController.Models.Send
{
    public class Category
    {
        public bool Deleted { get; set; }
        public string Name { get; set; }
        public Subcategory[] Subcategories { get; set; }

        public Category(){}

        public Category(Context.Category category)
        {
            Deleted = category.Deleted;
            Name = category.Name;
            Subcategories = category.Subcategories.Select(subcategory => new Subcategory(subcategory))
                .OrderBy(subcategory => subcategory.Name).ToArray();
        }
    }
}
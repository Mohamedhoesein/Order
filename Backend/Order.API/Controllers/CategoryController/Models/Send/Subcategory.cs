namespace Order.API.Controllers.CategoryController.Models.Send
{
    public class Subcategory
    {
        public bool Deleted { get; set; }
        public string Name { get; set; }

        public Subcategory() {}

        public Subcategory(Context.Subcategory subcategory)
        {
            Deleted = subcategory.Deleted;
            Name = subcategory.Name;
        }
    }
}
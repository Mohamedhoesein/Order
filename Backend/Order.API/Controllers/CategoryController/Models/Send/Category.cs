namespace Order.API.Controllers.CategoryController.Models.Send
{
    /// <summary>
    /// A category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// If the category is deleted.
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The subcategories of the category
        /// </summary>
        public Subcategory[] Subcategories { get; set; }

        /// <summary>
        /// Initialize a new <see cref="Category"/>.
        /// </summary>
        public Category(){}

        /// <summary>
        /// Initialize a new <see cref="Category"/> based on a <see cref="Context.Category"/>.
        /// </summary>
        /// <param name="category">
        /// The <see cref="Context.Category"/> to use.
        /// </param>
        public Category(Context.Category category)
        {
            Deleted = category.Deleted;
            Name = category.Name;
            Subcategories = category.Subcategories.Select(subcategory => new Subcategory(subcategory))
                .OrderBy(subcategory => subcategory.Name).ToArray();
        }
    }
}
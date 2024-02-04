namespace Order.API.Controllers.CategoryController.Models.Send
{
    /// <summary>
    /// A main category.
    /// </summary>
    public class MainCategory
    {
        /// <summary>
        /// If the main category is deleted.
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// The name of the main category.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The categories of the main category
        /// </summary>
        public Category[] Categories { get; set; }

        /// <summary>
        /// Initialize a new <see cref="MainCategory"/>.
        /// </summary>
        public MainCategory(){}

        /// <summary>
        /// Initialize a new <see cref="MainCategory"/> based on a <see cref="Context.MainCategory"/>.
        /// </summary>
        /// <param name="mainCategory">
        /// The <see cref="Context.MainCategory"/> to use.
        /// </param>
        public MainCategory(Context.MainCategory mainCategory)
        {
            Deleted = mainCategory.Deleted;
            Name = mainCategory.Name;
            Categories = mainCategory.Categories.Select(category => new Category(category))
                .OrderBy(category => category.Name).ToArray();
        }
    }
}
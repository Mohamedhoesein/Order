namespace Order.API.Controllers.CategoryController.Models.Receive
{
    /// <summary>
    /// A category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The associated subcategories.
        /// </summary>
        public string[] Subcategories { get; set; }
    }
}
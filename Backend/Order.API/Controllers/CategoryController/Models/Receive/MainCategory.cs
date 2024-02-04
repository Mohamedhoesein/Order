namespace Order.API.Controllers.CategoryController.Models.Receive
{
    /// <summary>
    /// A main category.
    /// </summary>
    public class MainCategory
    {
        /// <summary>
        /// The name of the main category.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The associated categories.
        /// </summary>
        public Category[] Categories { get; set; }
    }
}
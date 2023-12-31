using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A main category for the products.
    /// </summary>
    public class MainCategory
    {
        /// <summary>
        /// The name of the main category.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// If the main category is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The categories that fall under this main category.
        /// </summary>
        public ICollection<Category> Categories { get; set; }
    }
}
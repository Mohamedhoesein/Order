using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A category for the products.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// The name of the category.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// If the category is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The name of the main category that this category falls under.
        /// </summary>
        public string MainCategoryName { get; set; }
        /// <summary>
        /// The main category that this category falls under.
        /// </summary>
        public MainCategory MainCategory { get; set; }
        /// <summary>
        /// The sub categories that fall under this category.
        /// </summary>
        public ICollection<Subcategory> Subcategories { get; set; }
    }
}
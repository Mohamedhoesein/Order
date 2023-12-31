using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A sub category for the products.
    /// </summary>
    public class Subcategory
    {
        /// <summary>
        /// The name of the category.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// If the sub category is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The name of the category that this subcategory falls under.
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// The name of the main category this category falls under.
        /// </summary>
        public string MainCategoryName { get; set; }
        /// <summary>
        /// The category that this subcategory falls under.
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// The open specifications associated with this subcategory.
        /// </summary>
        public ICollection<OpenSpecification> OpenSpecifications { get; set; }
        /// <summary>
        /// The open specifications associated with this subcategory.
        /// </summary>
        public ICollection<ClosedSpecification> ClosedSpecifications { get; set; }
        /// <summary>
        /// The filters associated with this subcategory.
        /// </summary>
        public ICollection<Filter> Filters { get; set; }
        /// <summary>
        /// The products associated with this subcategory.
        /// </summary>
        public ICollection<Product> Products { get; set; }
    }
}
using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A filter for products.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// The title for the filter.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The name of the subcategory this filter is associated with.
        /// </summary>
        public string SubcategoryName { get; set; }
        /// <summary>
        /// The name of the category the subcategory is associated with.
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// The name of the main category the category is associated with.
        /// </summary>
        public string MainCategoryName { get; set; }
        /// <summary>
        /// The id of the subcategory this filter is associated with.
        /// </summary>
        public Subcategory Subcategory { get; set; }
        /// <summary>
        /// The name of the closed specification this filter is associated with.
        /// </summary>
        public string ClosedSpecificationName { get; set; }
        /// <summary>
        /// The closed specification this filter is associated with.
        /// </summary>
        public ClosedSpecification ClosedSpecification { get; set; }
    }
}
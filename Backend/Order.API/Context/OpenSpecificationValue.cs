using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// The value for a specific specification of a product, which has an open value. 
    /// </summary>
    public class OpenSpecificationValue
    {
        /// <summary>
        /// The value of the specification.
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// The name of the specification this value is associated with.
        /// </summary>
        public string SpecificationName { get; set; }
        /// <summary>
        /// The name of the subcategory the specification is associated with,
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
        /// The specification this value is associated with.
        /// </summary>
        public OpenSpecification OpenSpecification { get; set; }
        /// <summary>
        /// The name of the product this value is associated with.
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// The product this value is associated with.
        /// </summary>
        public Product Product { get; set; }
    }
}
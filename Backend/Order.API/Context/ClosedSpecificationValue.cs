using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A value for a closed specification.
    /// </summary>
    public class ClosedSpecificationValue
    {
        /// <summary>
        /// The value of the specification.
        /// </summary>
        [Required]
        public string Value { get; set; }
        /// <summary>
        /// If the specification value is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

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
        public ClosedSpecification ClosedSpecification { get; set; }
        /// <summary>
        /// The products this value is associated with.
        /// </summary>
        public ICollection<Product> Products { get; set; }
    }
}
using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// Specifications for a product which has a closed set of possible values.
    /// </summary>
    public class ClosedSpecification
    {
        /// <summary>
        /// The name of the specification.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// If the specification is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The name of the subcategory this specification is associated with.
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
        /// The subcategory this specification is associated with.
        /// </summary>
        public Subcategory Subcategory { get; set; }
        /// <summary>
        /// The values for products associated with this specification.
        /// </summary>
        public ICollection<ClosedSpecificationValue> ClosedSpecificationValues { get; set; }
        /// <summary>
        /// A filter associated with this specification. 
        /// </summary>
        public Filter? Filter { get; set; }
    }
}
using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The description of the product.
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// The price of the product in cents.
        /// </summary>
        [Required]
        public int Price { get; set; }
        /// <summary>
        /// If the product is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The id of the subcategory this product is associated with.
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
        /// The subcategory this product is associated with.
        /// </summary>
        public Subcategory Subcategory { get; set; }
        /// <summary>
        /// The values for open specifications.
        /// </summary>
        public ICollection<OpenSpecificationValue> OpenSpecificationValues { get; set; }
        /// <summary>
        /// The values for closed specifications.
        /// </summary>
        public ICollection<ClosedSpecificationValue> ClosedSpecificationValues { get; set; }
    }
}
using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The id of the product.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// If the product is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The name of the subcategory this product is associated with.
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
        /// The associated 
        /// </summary>
        public ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
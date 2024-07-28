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
        /// The id of the category this product is associated with.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// The category this product is associated with.
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// The associated 
        /// </summary>
        public ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
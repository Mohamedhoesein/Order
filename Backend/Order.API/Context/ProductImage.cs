using System.ComponentModel.DataAnnotations;

namespace Order.API.Context
{
    /// <summary>
    /// Images associated with a product.
    /// </summary>
    public class ProductImage
    {
        /// <summary>
        /// The name of the image.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The name of the file.
        /// </summary>
        [Required]
        public string File { get; set; }
        /// <summary>
        /// The id of the product.
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// The version of the product.
        /// </summary>
        public long ProductVersionNumber { get; set; }
        /// <summary>
        /// The associated product version.
        /// </summary>
        public ProductVersion ProductVersion { get; set; } 
    }
}
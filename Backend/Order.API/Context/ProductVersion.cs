using System.ComponentModel.DataAnnotations;

namespace Order.API.Context
{
    public class ProductVersion
    {
        /// <summary>
        /// The version number of the product.
        /// </summary>
        [Required]
        public long VersionNumber { get; set; }
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
        public ulong Price { get; set; }
        /// <summary>
        /// The id of the product.
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// The associated product.
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// The images for the product.
        /// </summary>
        public ICollection<ProductImage> ProductImages { get; set; }
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
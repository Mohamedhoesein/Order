using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// The value for a specific specification of a product, which has an open value. 
    /// </summary>
    public class OpenSpecificationValue
    {
        /// <summary>
        /// The id of the specification value.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The value of the specification.
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// The id of the specification this value is associated with.
        /// </summary>
        public int SpecificationId { get; set; }
        /// <summary>
        /// The specification this value is associated with.
        /// </summary>
        public OpenSpecification OpenSpecification { get; set; }
        /// <summary>
        /// The id of the product this value is associated with.
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// The version of the product this value is associated with.
        /// </summary>
        public long ProductVersionNumber { get; set; }
        /// <summary>
        /// The product this value is associated with.
        /// </summary>
        public ProductVersion ProductVersion { get; set; }
    }
}
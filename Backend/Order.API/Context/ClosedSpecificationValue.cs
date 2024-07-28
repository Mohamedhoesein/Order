using Microsoft.Build.Framework;

namespace Order.API.Context
{
    /// <summary>
    /// A value for a closed specification.
    /// </summary>
    public class ClosedSpecificationValue
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
        /// If the specification value is deleted.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The id of the specification this value is associated with.
        /// </summary>
        public int SpecificationId { get; set; }
        /// <summary>
        /// The specification this value is associated with.
        /// </summary>
        public ClosedSpecification ClosedSpecification { get; set; }
        /// <summary>
        /// The products this value is associated with.
        /// </summary>
        public ICollection<ProductVersion> ProductVersions { get; set; }
    }
}
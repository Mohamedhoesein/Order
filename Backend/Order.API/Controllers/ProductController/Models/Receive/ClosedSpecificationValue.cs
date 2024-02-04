namespace Order.API.Controllers.ProductController.Models.Receive
{
    /// <summary>
    /// A value for a closed specification.
    /// </summary>
    public class ClosedSpecificationValue
    {
        /// <summary>
        /// The name of the specification.
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// The value of the specification.
        /// </summary>
        public string Value { get; set; }
    }
}
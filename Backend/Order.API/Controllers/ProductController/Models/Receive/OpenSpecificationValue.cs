namespace Order.API.Controllers.ProductController.Models.Receive
{
    /// <summary>
    /// A value for an open specification.
    /// </summary>
    public class OpenSpecificationValue
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
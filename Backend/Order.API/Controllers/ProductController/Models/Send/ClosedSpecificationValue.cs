namespace Order.API.Controllers.ProductController.Models.Send
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

        /// <summary>
        /// Initialize a new empty <see cref="ClosedSpecificationValue"/>.
        /// </summary>
        public ClosedSpecificationValue() {}

        /// <summary>
        /// Initialize a new <see cref="ClosedSpecificationValue"/> based on a <see cref="Context.ClosedSpecificationValue"/>.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Context.ClosedSpecificationValue"/> to use.
        /// </param>
        public ClosedSpecificationValue(Context.ClosedSpecificationValue value)
        {
            Specification = value.SpecificationName;
            Value = value.Value;
        }
    }
}
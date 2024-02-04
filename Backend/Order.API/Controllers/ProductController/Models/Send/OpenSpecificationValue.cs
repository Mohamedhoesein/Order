namespace Order.API.Controllers.ProductController.Models.Send
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

        /// <summary>
        /// Initialize a new empty <see cref="OpenSpecificationValue"/>.
        /// </summary>
        public OpenSpecificationValue() {}

        /// <summary>
        /// Initialize a new <see cref="OpenSpecificationValue"/> based on a <see cref="Context.OpenSpecificationValue"/>.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Context.OpenSpecificationValue"/> to use.
        /// </param>
        public OpenSpecificationValue(Context.OpenSpecificationValue value)
        {
            Specification = value.SpecificationName;
            Value = value.Value;
        }
    }
}
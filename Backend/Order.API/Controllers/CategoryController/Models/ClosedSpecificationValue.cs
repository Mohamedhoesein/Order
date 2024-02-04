namespace Order.API.Controllers.CategoryController.Models
{
    /// <summary>
    /// A value for a closed specification.
    /// </summary>
    public class ClosedSpecificationValue
    {
        /// <summary>
        /// The value of the specification.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// If the value is deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Initialize a new <see cref="ClosedSpecificationValue"/>.
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
            Value = value.Value;
            Deleted = value.Deleted;
        }
    }
}
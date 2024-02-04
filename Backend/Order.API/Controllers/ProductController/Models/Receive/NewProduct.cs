namespace Order.API.Controllers.ProductController.Models.Receive
{
    /// <summary>
    /// A new product.
    /// </summary>
    public class NewProduct
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the product.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The price of the product in cents.
        /// </summary>
        public ulong Price { get; set; }
        /// <summary>
        /// If the product is deleted.
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// The associated closed specification values.
        /// </summary>
        public ClosedSpecificationValue[]? ClosedSpecificationValues { get; set; }
        /// <summary>
        /// The associated open specification values.
        /// </summary>
        public OpenSpecificationValue[]? OpenSpecificationValues { get; set; }
        /// <summary>
        /// The images of the product.
        /// </summary>
        public NewImage[]? Images { get; set; }
    }
}
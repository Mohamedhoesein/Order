namespace Order.API.Controllers.ProductController.Models.Receive
{
    /// <summary>
    /// A new image for a product.
    /// </summary>
    public class NewImage
    {
        /// <summary>
        /// The name of the image.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The file information.
        /// </summary>
        public IFormFile File { get; set; }
    }
}
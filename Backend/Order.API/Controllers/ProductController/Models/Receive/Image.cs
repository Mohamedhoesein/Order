namespace Order.API.Controllers.ProductController.Models.Receive
{
    /// <summary>
    /// An image for a product.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// The name for the image.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The file information.
        /// </summary>
        public IFormFile? File { get; set; }
    }
}
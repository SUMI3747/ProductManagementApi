using PrdouctsApi.Models;

namespace PrdouctsApi.ProductServices
{
    public class ProductRequestResponse
    {
        public string? statusMessage { get; set; }
        public required ProductsModel productDetails { get; set; }
    }
}

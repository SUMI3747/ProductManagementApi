using PrdouctsApi.Models;

namespace PrdouctsApi.ProductServices
{
    public interface IProductService
    {
        Task<ProductRequestResponse> AddProductAsync(string productName, int stockAvailable);

        Task<List<ProductsModel>> GetAllProductsAsync();

        Task<ProductsModel> GetProductByIdAsync(string productId);

        Task<bool> DeleteProductByIdAsync(string productId);

        Task<ProductRequestResponse> UpdateProductAsync(string id, ProductDto productDto);

        Task<ProductRequestResponse> DecrementStockAsync(string productId, int quantity);

        Task<ProductRequestResponse> IncrementStockAsync(string productId, int quantity);

    }
}
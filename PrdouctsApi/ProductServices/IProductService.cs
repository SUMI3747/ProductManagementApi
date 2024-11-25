using PrdouctsApi.Models.DTOs;
using PrdouctsApi.Models.Entities;

namespace PrdouctsApi.ProductServices
{
    public interface IProductService
    {
        Task<ProductRequestResponse> AddProductAsync(string productName, int stockAvailable);

        Task<List<Products>> GetAllProductsAsync();

        Task<Products> GetProductByIdAsync(int productId);

        Task<bool> DeleteProductByIdAsync(int productId);

        Task<ProductRequestResponse> UpdateProductAsync(int productId, ProductDto productDto);

        Task<ProductRequestResponse> DecrementStockAsync(int productId, int quantity);

        Task<ProductRequestResponse> IncrementStockAsync(int productId, int quantity);

    }
}
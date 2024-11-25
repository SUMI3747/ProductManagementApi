using PrdouctsApi.Models.DTOs;

namespace ProductInventoryManagerAPI.ProductServices
{
    public interface IStockUpdateService
    {
        Task<ProductRequestResponse> ModifyStock(int productId, int quantity, bool isIncrement);
    }
}


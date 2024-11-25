using Microsoft.EntityFrameworkCore;
using PrdouctsApi.Data;
using PrdouctsApi.Models.DTOs;

namespace ProductInventoryManagerAPI.ProductServices
{
    public class StockUpdateService : IStockUpdateService
    {
        private readonly ProductDbContext _context;
        private readonly ProductRequestResponse _respObject;

        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public StockUpdateService(ProductDbContext context, ProductRequestResponse respObject)
        {
            _context = context;
            _respObject = respObject;
        }

        public async Task<ProductRequestResponse> ModifyStock(int ProductID, int quantity, bool isIncrement)
        {
            await _semaphore.WaitAsync();

            try
            { 
                var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == ProductID);

                if (product == null)
                {
                    _respObject.statusMessage = "Product not found";
                    return _respObject;
                }

              
                if (!isIncrement && product.StockAvailable < quantity)
                {
                    _respObject.statusMessage = "Insufficient stock available. Please select a quantity within the available stock.";
                    _respObject.productDetails = product;
                    return _respObject;

                }

                // Modify the stock based on the InCrement/Decrement operation
                product.StockAvailable = isIncrement ? product.StockAvailable + quantity : product.StockAvailable - quantity;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                _respObject.statusMessage = isIncrement ? "Stock Incremented successfully" : "Stock Decremented successfully";
                _respObject.productDetails = product;
                return _respObject;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}


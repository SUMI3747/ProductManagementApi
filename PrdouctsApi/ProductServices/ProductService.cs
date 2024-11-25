using Microsoft.EntityFrameworkCore;
using PrdouctsApi.Data;
using PrdouctsApi.Models.DTOs;
using PrdouctsApi.Models.Entities;
using ProductInventoryManagerAPI.ProductServices;

namespace PrdouctsApi.ProductServices
{
    public class ProductService(ProductDbContext context, IStockUpdateService stockUpdateService,
        ProductRequestResponse respObject, Products productObject) : IProductService
    {
        private readonly ProductDbContext _context = context;
        private readonly IStockUpdateService _stockUpdateService = stockUpdateService;

       
        public async Task<ProductRequestResponse> AddProductAsync(string productName, int stockAvailable)
        {
            Products existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == productName);

            if (existingProduct != null)
            {
                respObject.statusMessage = "Product Already Exist, Please Update Stock Quantity Only";
                respObject.productDetails = existingProduct;
                return respObject;
            }
            else
            {
                // Create a new product object
                productObject.ProductName = productName;
                productObject.StockAvailable = stockAvailable;

                await _context.Products.AddAsync(productObject);
                await _context.SaveChangesAsync();

                respObject.statusMessage = "New Product Added successfully";
                respObject.productDetails = productObject;

                return respObject;
            }
        }
     
        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Products> GetProductByIdAsync(int productId)
        {
            return await _context.Products.SingleOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<bool> DeleteProductByIdAsync(int productId)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ProductRequestResponse> UpdateProductAsync(int productId, ProductDto productDto)
        {

            var existingProduct = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == productId);

            if (existingProduct == null)
            {
                respObject.statusMessage = "Product not found";
                return respObject;
            }

            existingProduct.ProductName = productDto.ProductName ?? existingProduct.ProductName;
            existingProduct.StockAvailable = productDto.StockAvailable;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
           
            respObject.statusMessage = "Product updated successfully";
            respObject.productDetails = existingProduct;
            return respObject;
        }

        //Decrement Product Stock Quantity
        public Task<ProductRequestResponse> DecrementStockAsync(int productId, int quantity)
        {
            return _stockUpdateService.ModifyStock(productId, quantity, isIncrement: false);
        }

        // Increment Product stock Quantity
        public Task<ProductRequestResponse> IncrementStockAsync(int productId, int quantity)
        {
            return _stockUpdateService.ModifyStock(productId, quantity, isIncrement: true);
        }

    }
}


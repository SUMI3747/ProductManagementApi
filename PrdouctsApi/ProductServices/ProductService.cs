using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrdouctsApi.Models;
namespace PrdouctsApi.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateProductIdAsync()
        {

            // Fetch the last productID from the database
            string maxProductId = await _context.Products
                .OrderByDescending(p => p.productID)
                .Select(p => p.productID)
                .FirstOrDefaultAsync();

            // Default to "000001" if no products exist
            int newProductIdNumber = 1;

            // If a product exists, increment the last product ID
            if (!string.IsNullOrEmpty(maxProductId) && int.TryParse(maxProductId, out int lastProductId))
            {
                newProductIdNumber = lastProductId + 1;
            }

            // Ensure the product ID is always 6 digits (e.g., 000001, 000002, etc.)
            return newProductIdNumber.ToString("D6");
        }

        // Method to add a new product to the database
        public async Task<ProductRequestResponse> AddProductAsync(string productName, int stockAvailable)
        { 
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.productName == productName);

            if (existingProduct != null)
            {
                return new ProductRequestResponse
                {
                    statusMessage = "Product Already Exist, Please Update Stock Quantity Only",
                    productDetails = new ProductsModel
                    {
                        productID = existingProduct.productID,
                        productName = existingProduct.productName,
                        stockAvailable = existingProduct.stockAvailable
                    }
                };

            }
            else
            {
                // Generate the next product ID
                string newProductId = await GenerateProductIdAsync();

                // Create a new product object
                var newProduct = new ProductsModel
                {
                    productID = newProductId, // Assign the generated product ID
                    productName = productName,
                    stockAvailable = stockAvailable
                };

                // Add the new product to the context and save changes
                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                return new ProductRequestResponse
                {
                    statusMessage = "New Product Added successfully.",
                    productDetails = new ProductsModel
                    {
                        productID = newProduct.productID,
                        productName = newProduct.productName,
                        stockAvailable = newProduct.stockAvailable
                    }
                };
            }
        }

        public async Task<List<ProductsModel>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductsModel> GetProductByIdAsync(string productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.productID == productId);
        }

        public async Task<bool> DeleteProductByIdAsync(string productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.productID == productId);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ProductRequestResponse> UpdateProductAsync(string id, ProductDto productDto)
        {
           
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.productID == id);

            if (existingProduct == null)
            {
                return new ProductRequestResponse
                {
                    statusMessage = "Product not found",
                    productDetails = null
                };
            }

            existingProduct.productName = productDto.ProductName ?? existingProduct.productName;
            existingProduct.stockAvailable = productDto.StockAvailable != 0 ? existingProduct.stockAvailable + productDto.StockAvailable : existingProduct.stockAvailable;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return new ProductRequestResponse
            {
                statusMessage = "Product updated successfully",
                productDetails = new ProductsModel
                {
                     productID=existingProduct.productID,
                     productName=existingProduct.productName,
                    stockAvailable= existingProduct.stockAvailable
                }
            };
        }



        public async Task<ProductRequestResponse> DecrementStockAsync(string productId, int quantity)
        {
            Thread.Sleep(2000);
            await _semaphore.WaitAsync(); // Wait for the semaphore to be available

            try
            {
                // Fetch the product by ID
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.productID == productId);

                if (product == null)
                {
                    return new ProductRequestResponse
                    {
                        statusMessage = "Product not found",
                        productDetails = null
                    };
                }

                // Check if enough stock is available
                if (product.stockAvailable < quantity)
                {
                    return new ProductRequestResponse
                    {
                        statusMessage = "Insufficient stock available. Please select a quantity within the available stock.",
                        productDetails = new ProductsModel
                        {
                            productID = product.productID,
                            productName = product.productName,
                            stockAvailable = product.stockAvailable
                        }
                    };
                }

                // Decrement stock
                product.stockAvailable -= quantity;

                // Update the product in the database
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return new ProductRequestResponse
                {
                    statusMessage = "Stock decremented successfully",
                    productDetails = new ProductsModel
                    {
                        productID = product.productID,
                        productName = product.productName,
                        stockAvailable = product.stockAvailable
                    }
                };
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur during the operation
                return new ProductRequestResponse
                {
                    statusMessage = $"An error occurred: {ex.Message}",
                    productDetails = null
                };
            }
            finally
            {
                _semaphore.Release(); // Release the semaphore, allowing the next thread to proceed
            }

        }

        public async Task<ProductRequestResponse> IncrementStockAsync(string productId, int quantity)
        {
            Thread.Sleep(2000);
            await _semaphore.WaitAsync(); // Wait for the semaphore to be available

            try
            {
                // Fetch the product by ID
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.productID == productId);

                if (product == null)
                {
                    return new ProductRequestResponse
                    {
                        statusMessage = "Product not found",
                        productDetails = null
                    };
                }

             
                // Decrement stock
                product.stockAvailable += quantity;

                // Update the product in the database
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return new ProductRequestResponse
                {
                    statusMessage = "Stock Incremented successfully",
                    productDetails = new ProductsModel
                    {
                        productID = product.productID,
                        productName = product.productName,
                        stockAvailable = product.stockAvailable
                    }
                };
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur during the operation
                return new ProductRequestResponse
                {
                    statusMessage = $"An error occurred: {ex.Message}",
                    productDetails = null
                };
            }
            finally
            {
                _semaphore.Release(); // Release the semaphore, allowing the next thread to proceed
            }

        }

    }
    }


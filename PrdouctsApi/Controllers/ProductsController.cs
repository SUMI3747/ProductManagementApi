/*Purpose: API operations for product management | Author: Sumit L | Date: 24th Nov 2024 | For training and learning purposes*/

using Microsoft.AspNetCore.Mvc;
using PrdouctsApi.Helpers;
using PrdouctsApi.Models.DTOs;
using PrdouctsApi.ProductServices;

namespace PrdouctsApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductValidationHelper _validationHelper;
        private readonly IServiceResponseHelper _responseHelper;

        public ProductsController(IProductService productService, 
            IProductValidationHelper validationHelper,
            IServiceResponseHelper responseHelper)
        {
            _productService = productService;
            _validationHelper = validationHelper;
            _responseHelper = responseHelper;
        }

        
        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductDto productDto)
        { 
            var validationResult = _validationHelper.ValidateProductDto(productDto);
            if (validationResult != null) return validationResult;

            var response = await _productService.AddProductAsync(productDto.ProductName, productDto.StockAvailable);
            return _responseHelper.HandleServiceResponse(response, "An error occurred while adding the product.");
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _productService.DeleteProductByIdAsync(id);
            return isDeleted
                ? Ok($"Product with ID {id} deleted successfully.")
                : NotFound($"Product with ID {id} not found.");
        }

        /// <summary>
        /// Updates a product's details.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }
            var validationResult = _validationHelper.ValidateProductDto(productDto);
            if (validationResult != null) return validationResult;

            var response = await _productService.UpdateProductAsync(id, productDto);
            return _responseHelper.HandleServiceResponse(response, $"Product with ID {id} not found.");
        }

        /// <summary>
        /// Decrements a product's stock quantity.
        /// </summary>
        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<ActionResult> DecrementStock(int id, int quantity)
        {
            if (quantity <= 0) return BadRequest("Quantity must be greater than 0.");

            var response = await _productService.DecrementStockAsync(id, quantity);
            return _responseHelper.HandleServiceResponse(response, $"Product with ID {id} not found.");
        }

        /// <summary>
        /// Increments a product's stock quantity.
        /// </summary>
        [HttpPut("increment-stock/{id}/{quantity}")]
        public async Task<ActionResult> IncrementStock(int id, int quantity)
        {
            if (quantity <= 0) return BadRequest("Quantity must be greater than 0.");

            var response = await _productService.IncrementStockAsync(id, quantity);
            return _responseHelper.HandleServiceResponse(response, $"Product with ID {id} not found.");
        }
    }
}

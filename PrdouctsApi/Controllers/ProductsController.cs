using Microsoft.AspNetCore.Mvc;
using PrdouctsApi.Models;
using PrdouctsApi.ProductServices;


namespace PrdouctsApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController( IProductService productService)
        {
            _productService = productService;
        }

        // POST: api/Products  --> Update Productname and StockQuantity in Body as Request
        [HttpPost]
        public async Task<IActionResult> AddProducts([FromBody] ProductDto productDto)
        {
            if (string.IsNullOrEmpty(productDto.ProductName) || productDto.StockAvailable <= 0)
            {
                return BadRequest("Invalid Body Data please Check");
            }
            ProductRequestResponse productUpdateStatus = await _productService.AddProductAsync(productDto.ProductName, productDto.StockAvailable);

            return Ok(new
            {
                Message = productUpdateStatus.statusMessage,
                Product = new
                {
                   productUpdateStatus.productDetails.productID,
                   productUpdateStatus.productDetails.productName,
                   productUpdateStatus.productDetails.stockAvailable
                }
            });
        }


        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }

        // GET api/<ProductsController>/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }


        //DELETE api/<ProductsController>/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            var isDeleted = await _productService.DeleteProductByIdAsync(id);

            if (!isDeleted)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok($"Product with ID {id} deleted successfully.");
        }

        // PUT api/<products>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductById(string id, [FromBody] ProductDto productDto)
        { 
            var response = await _productService.UpdateProductAsync(id, productDto);
            return Ok(response);

        }

        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementStockAsync(string id, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest(new { message = "Quantity must be greater than 0." });
            }

            var response = await _productService.DecrementStockAsync(id, quantity);

            if (response.statusMessage == "Product not found")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPut("Increment-stock/{id}/{quantity}")]
        public async Task<IActionResult> IncrementStockAsync(string id, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest(new { message = "Quantity must be greater than 0." });
            }

            var response = await _productService.IncrementStockAsync(id, quantity);

            if (response.statusMessage == "Product not found")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}

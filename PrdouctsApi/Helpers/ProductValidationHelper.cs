using Microsoft.AspNetCore.Mvc;
using PrdouctsApi.Models.DTOs;

namespace PrdouctsApi.Helpers
{
    public class ProductValidationHelper : IProductValidationHelper
    {
        public ActionResult ValidateProductDto(ProductDto productDto)
        {
            if (productDto == null)
            {
                return new BadRequestObjectResult("Product data cannot be null");
            }

            if (string.IsNullOrWhiteSpace(productDto.ProductName))
            {
                return new BadRequestObjectResult("Product name cannot be empty");
            }

            if (productDto.StockAvailable <= 0)
            {
                return new BadRequestObjectResult("Enter Stock Quantity must be greater than 0");
            }

            return null; // Valid
        }
    }
}


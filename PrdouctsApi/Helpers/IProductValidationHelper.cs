using Microsoft.AspNetCore.Mvc;
using PrdouctsApi.Models.DTOs;

namespace PrdouctsApi.Helpers
{
    public interface IProductValidationHelper
    {
        ActionResult ValidateProductDto(ProductDto productDto);
    }
}

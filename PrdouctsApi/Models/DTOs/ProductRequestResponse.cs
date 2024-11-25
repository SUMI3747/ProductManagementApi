using PrdouctsApi.Models.Entities;
using ProductInventoryManagerAPI.Models.Entities;

namespace PrdouctsApi.Models.DTOs
{
    public class ProductRequestResponse
    {
        public string? statusMessage { get; set; }
        public required IProducts productDetails { get; set; }
    }
}

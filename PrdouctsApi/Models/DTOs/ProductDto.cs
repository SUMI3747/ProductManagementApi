using System.ComponentModel.DataAnnotations;

namespace PrdouctsApi.Models.DTOs
{
    public class ProductDto
    {
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "StockAvailable is required.")]
        public int StockAvailable { get; set; }
    }
}

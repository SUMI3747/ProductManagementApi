using System.ComponentModel.DataAnnotations;

namespace PrdouctsApi.Models
{
    public class ProductDto
    {
      
        public string? ProductName { get; set; }


        public int StockAvailable { get; set; }
    }

}

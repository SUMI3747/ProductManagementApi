using ProductInventoryManagerAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrdouctsApi.Models.Entities
{
    public class Products: IProducts
    {
        [Required]
        [Column("productID")]
        public int ProductId { get; set; }
        [Column("productName")]
        public string? ProductName { get; set; }
        [Column("stockAvailable")]
        public int StockAvailable { get; set; }
    }
}

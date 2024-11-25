using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductInventoryManagerAPI.Models.Entities
{
    public interface IProducts
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public int StockAvailable { get; set; }
    }
}

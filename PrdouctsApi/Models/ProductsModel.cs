using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrdouctsApi.Models
{
    public class ProductsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [MaxLength(6)]
        public string? productID { get; set; }

        public string? productName { get; set; }

        public int stockAvailable { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Butique.Models.Entity
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Category.Id))]
        public int CategoryId { get; set; }
        public virtual Category? CategoryEntity { get; set; }
        [Required]
        [MinLength(10)]
        public string? Name { get; set; }
        [RegularExpression(@"^[A-Z]{2}[0-9]{4}$",ErrorMessage ="Product Code is invalid")]
        public string? Code { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FishStore.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual OrderHeader OrderHeader { get; set; }

        [Required]
        public int StoreItemId { get; set; }

        [ForeignKey("StoreItemId")]
        public virtual StoreItem StoreItem { get; set; }

        public int Count { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }
    }
}

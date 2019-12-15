using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishStore.Models
{
    public class StoreItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Image { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }


        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

        [Range(0,int.MaxValue,ErrorMessage ="Value should be greater than 0")]
        public double Volume { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
    }
}

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

        [Display(Name = "Nome")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Descrição")]
        [Required]
        public string Description { get; set; }

        public string Image { get; set; }

        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }


        [Display(Name = "Subcategoria")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Valor deve ser maior que 0")]
        [Display(Name = "Volume(m³)")]
        public double Volume { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Valor deve ser maior que 0")]
        [Display(Name = "Massa(g)")]
        public double Weight { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Valor deve ser maior que 0")]
        [Display(Name = "Preço(R$)")]
        public double Price { get; set; }
    }
}

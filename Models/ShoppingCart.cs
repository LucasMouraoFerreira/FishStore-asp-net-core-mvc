using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishStore.Models
{
    public class ShoppingCart
    {

        public ShoppingCart()
        {
            Count = 1;
        }

        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        
        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int StoreItemId { get; set; }

        [NotMapped]
        [ForeignKey("StoreItemId")]
        public virtual StoreItem StoreItem { get; set; }

        [Display(Name ="Quantidade")]
        [Range(1,int.MaxValue, ErrorMessage = "O valor deve ser igual ou maior a [1]")]
        public int Count { get; set; }
    }
}

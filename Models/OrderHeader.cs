using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishStore.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public double OrderTotalOriginal { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Total")]
        public double OrderTotal { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Esse campo deve conter 8 dígitos!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O CEP deve conter apenas dígitos!")]
        [Display(Name = "CEP")]
        public string PostalCode { get; set; }

        public double PostalPrice { get; set; }

        public double PostalTime { get; set; }
        
        public string Status { get; set; }
        
        public string PaymentStatus { get; set; }
        
        public string TransactionId { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Endereço")]
        public string StreetAddress { get; set; }
        [Display(Name = "Cidade")]
        public string City { get; set; }
        [Display(Name = "Estado")]
        public string State { get; set; }
        [Display(Name = "CPE")]
        public string PostalCode { get; set; }
    }
}

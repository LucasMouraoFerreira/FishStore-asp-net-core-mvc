using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishStore.Models.ViewModels
{
    public class ProductsViewModel
    {
        public IEnumerable<StoreItem> StoreItem { get; set; }
        public IEnumerable<Category> Category { get; set; }
    }
}

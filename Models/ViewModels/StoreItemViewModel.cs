using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishStore.Models.ViewModels
{
    public class StoreItemViewModel
    {
        public StoreItem StoreItem { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<SubCategory> SubCategory { get; set; }
    }
}

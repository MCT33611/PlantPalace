using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models.ViewModels
{
    public class ProductListVM
    {
        public List<Product> products { get; set; }
        public List<Category> Categories { get; set; }
        public int SelectedCategoryId { get; set; }

    }
}

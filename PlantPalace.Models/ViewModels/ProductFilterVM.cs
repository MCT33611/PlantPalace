using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models.ViewModels
{
    public class ProductFilterVM
    {

        public string? Rates { get; set; }
        public string? Categories { get; set; }
        
        public string? search { get;set; }

        public int? PriceRange { get; set; }
    }
}

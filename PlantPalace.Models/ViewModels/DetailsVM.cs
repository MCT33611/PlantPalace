using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models.ViewModels
{
    public class DetailsVM
    {
        public ShoppingCart? cart { get; set; }
        public ProductReview? review { get; set; }

        public bool IsEligibleToReview { get; set; }
    }
}

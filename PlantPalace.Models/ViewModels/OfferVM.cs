using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models.ViewModels
{
    public class OfferVM
    {


        [ValidateNever]
        public List<Product> Products { get; set; }
        [ValidateNever]
        public Offer Offer { get; set; }
    }
}

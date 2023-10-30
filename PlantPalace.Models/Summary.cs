using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantPalace.Models.ViewModels;

namespace PlantPalace.Models
{
    public class Summary
    {
        public int Id { get; set; }

        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser User { get; set; }  

        public ShoppingCart shoppingCart{ get; set; }
    }
}

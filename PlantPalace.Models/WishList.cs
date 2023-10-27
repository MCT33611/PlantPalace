using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models
{
    public class WishList
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ValidateNever]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public string userId { get; set; }
        [ValidateNever]
        [ForeignKey("userId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}

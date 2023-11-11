using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int categoryId { get; set; }


        [Required]
        [ValidateNever]
        [ForeignKey("categoryId")]
        public Category Category { get; set; }

        [Required]
        [MaxLength(30,ErrorMessage ="Maximun charecters are 30")]
        public string name { get; set; }


        [ValidateNever]
        public string ImageUrl { get; set; }



    }
}

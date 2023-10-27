using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PlantPalace.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters for Name is 30")]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [MaxLength(1000, ErrorMessage = "Maximum characters for Description is 255")]
        public string Description { get; set; }

       /* [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number")]
        public int Stock { get; set; }*/

        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }


        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        [Display(Name = "Price for 1 - 50")]
        public double Price { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        [Display(Name = "Price for 50+")]
        public double Price50 { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        [Display(Name = "Price for 100+")]
        public double Price100 { get; set; }
    /*
        [Range(0.0, 100.0, ErrorMessage = "Discount must be between 0 and 100")]
        public double Discount { get; set; }*/

        [Required(ErrorMessage = "Category ID is required")]
        public int categoryId { get; set; }

        [ForeignKey("categoryId")]
        [ValidateNever]

        public Category Category { get; set; }

        [ValidateNever]

        public string ImageUrl { get; set; }

		[ValidateNever]
		public string? ImageOne { get; set; }
        [ValidateNever]
        public string? ImageTwo { get; set; }
        [ValidateNever]
        public string? ImageThree { get; set; }


        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        [Required]
        public int Stock { get;set; }
    }

}

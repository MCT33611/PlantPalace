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
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        /*[Required]
        public int ProductId { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }*/

        [Required]
        [RegularExpression(@"\.(jpg|jpeg|png|gif|bmp|svg)$", ErrorMessage = "Invalid image file extension. Supported extensions are jpg, jpeg, png, gif, bmp, svg.")]
        [ValidateNever]
        public string? ImageUrl { get; set; }

        [Required]
        public string? OfferName { get; set;}

        [Required]
        [Range(1, 99,ErrorMessage ="Please enter 1 to 99 ")]
        public double OfferPercent { get; set; }


        [Required]
        public string OfferType { get; set; }

        [Required]
        [ValidateNever]
        public string? OfferUrl { get; set; }

        public DateTime AddedDate { get; set; }
        public string? Description { get; set; }

        
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"\.(jpg|jpeg|png|gif|bmp|svg)$", ErrorMessage = "Invalid image file extension. Supported extensions are jpg, jpeg, png, gif, bmp, svg.")]
        [ValidateNever]
        public string? ImageUrl { get; set; }

        [Required]
        public string? BannerName { get; set;}

        [Required]
        [ValidateNever]
        public string? BannerUrl { get; set; }

        public DateTime AddedDate { get; set; }
        public string? Description { get; set; }

        [Required]
        public bool IsBanned { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models
{
    public class ProductReview
    {
        [Key]
        public int Id { get; set; }

        [AllowNull]
        public string? UserId { get; set; }

        [AllowNull]
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        [Required(ErrorMessage = "Please provide a rating.")]
        [Range(0, 5, ErrorMessage = "The rating must be between 1 and 5.")]
        public int Rate { get; set; }


        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models
{
    public class WalletTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string userId { get; set; }

        [ForeignKey(nameof(userId))]
        public ApplicationUser User { get; set; }

        [Required]
        [Range(double.MinValue, double.MaxValue)]
        public double Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}

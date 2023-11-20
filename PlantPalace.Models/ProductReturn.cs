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
    public class ProductReturn
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int OrderDetailId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(OrderDetailId))]
        public OrderDetail OrderDetail { get; set; }

        [Required]
        public string ReturnStatus { get; set; }

        [Required]
        public string ReturnReason { get; set; }
    }
}

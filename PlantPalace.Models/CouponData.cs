using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models
{
    public class CouponData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }


        [Required]
        public int CouponId { get; set; }
        [ForeignKey(nameof(CouponId))]
        public Coupon Coupon { get; set; }


        [Required]
        public DateTime Date { get; set; }

    }
}

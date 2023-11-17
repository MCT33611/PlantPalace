using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models
{
	public class Coupon
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? Name { get; set; }

		[Required]
		public string? Code { get; set; }

        [Required]
        public double? MinPrice { get; set; }

		[AllowNull]
        public int? Percent { get; set; }
		[AllowNull]
        public int? FixPrice { get; set; }

		[Required]
        public DateTime PublishDate { get; set; }

		[Required]
		public DateTime ExpiryDate { get; set; }

        [Required]
        public bool? IsPublic { get; set; }
    }
}

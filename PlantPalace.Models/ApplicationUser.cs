using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string? Name { get; set; }

        public string? StreetAdderss { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        public DateTime joinDate { get; set; } = DateTime.Now;


        [AllowNull]
        [Range(0,double.MaxValue)]
        public double WalletBalance { get; set; }

        [NotMapped]
        public string Role { get; set; }

        [AllowNull]
        public string? Pic { get; set; }
    }
}

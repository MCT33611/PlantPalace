using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models.ViewModels
{
    public class CouponVM
    {

        public List<Coupon> Coupons { get; set; }
        public List<CouponData> CouponsData { get; set; }
        public ApplicationUser user { get; set; }
    }
}

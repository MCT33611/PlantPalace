using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models.ViewModels
{
    public class DashboardVM
    {
        public IEnumerable<ApplicationUser> users { get; set; }

        public IEnumerable<Product> products { get; set; }

        public IEnumerable<OrderHeader> orders { get; set; }

        public IEnumerable<OrderDetail> ordersDetail { get; set; }

        public IEnumerable<ProductReview> productReviews { get; set; }

    }
}

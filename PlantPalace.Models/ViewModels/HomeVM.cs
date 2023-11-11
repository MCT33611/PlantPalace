using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Banner> banners { get; set; }

        public IEnumerable<Product> products { get; set; }

        public IEnumerable<Category> categories { get; set; }

        public IEnumerable<OrderDetail> orderDetails { get; set; }
    }
}

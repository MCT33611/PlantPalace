using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.Models.ViewModels
{
	public class OrderVM
	{
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetail> OrderDetail { get; set; }
        public IEnumerable<ProductReturn> ProductReturnList { get; set; }
    }
}

using PlantPalace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.DataAccess.Repository.IRepository
{
    public interface IProductReturnRepository : IRepository<ProductReturn>
    {

		void UpdateStatus(int id, string orderStatus);




    }
}
 
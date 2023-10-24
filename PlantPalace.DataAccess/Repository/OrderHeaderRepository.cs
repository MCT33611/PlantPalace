using PlantPalace.DataAccess.Data;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeader.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderDb = _db.OrderHeader.FirstOrDefault(u => u.Id == id);
            if (orderDb != null)
            {
                orderDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessonId, string paymentItentId)
        {
            var orderDb = _db.OrderHeader.FirstOrDefault(u => u.Id == id);
            orderDb.OrderDate = DateTime.Now;
            orderDb.SessionId = sessonId;
            orderDb.PaymentIntentId = paymentItentId;

        }
    }
}

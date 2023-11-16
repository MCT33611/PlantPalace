using PlantPalace.DataAccess.Data;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;

namespace PlantPalace.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private  ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser applicationUser)
        {
            _db.ApplicationUsers.Update(applicationUser);
        }

        public void UpdateWallet(string userId, double amount)
        {
            try
            {
                var user = _db.ApplicationUsers.Find(userId);

                if (user != null)
                {
                    user.WalletBalance += amount;
                }
                _db.ApplicationUsers.Update(user);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()+"______User not find here");
            }
        }
    }
}

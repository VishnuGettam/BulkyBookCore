using BulkyBook.DataAccess.Data;
using BulkyBook.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class AppUserRepository : Repository<ApplicationUser>, IAppUserRepository
    {
        private readonly ApplicationDbContext _db;

        public AppUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public void Update(ApplicationUser applicationUser)
        //{
        //    var objFromDB = _db.ApplicationUsers.FirstOrDefault(m => m.Id == applicationUser.Id);

        //    if (objFromDB != null)
        //    {
        //        _db.Entry<ApplicationUser>(objFromDB).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        //        objFromDB.UserName = applicationUser.UserName;
        //        objFromDB.Email = applicationUser.Email;
        //        objFromDB.City = applicationUser.City;
        //        objFromDB.PhoneNumber = applicationUser.PhoneNumber;
        //        objFromDB.PostalCode = applicationUser.PostalCode;
        //        objFromDB.State = applicationUser.State;
        //        objFromDB.StreetAddress = applicationUser.StreetAddress;

        //        _db.SaveChanges();
        //    }
        //}
    }
}
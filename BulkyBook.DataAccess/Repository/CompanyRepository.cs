using BulkyBook.DataAccess.Data;
using BulkyBook.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            var objFromDB = _db.Companies.FirstOrDefault(c => c.Id == company.Id);

            if (objFromDB != null)
            {
                _db.Entry<Company>(objFromDB).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                objFromDB.Name = company.Name;
                objFromDB.City = company.City;
                objFromDB.IsAuthorizedCompany = company.IsAuthorizedCompany;
                objFromDB.PhoneNumber = company.PhoneNumber;
                objFromDB.PostalCode = company.PostalCode;
                objFromDB.State = company.State;
                objFromDB.StreetAddress = company.StreetAddress;

                _db.SaveChanges();
            }
        }
    }
}
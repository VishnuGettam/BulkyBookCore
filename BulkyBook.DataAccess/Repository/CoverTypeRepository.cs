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
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CoverType coverType)
        {
            var objFromDB = _db.CoverTypes.FirstOrDefault(m => m.Id == coverType.Id);

            if (objFromDB != null)
            {
                _db.Entry<CoverType>(objFromDB).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                objFromDB.Name = coverType.Name;
                _db.SaveChanges();
            }
        }
    }
}
using BulkyBook.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly ApplicationDbContext _db;

        public UnitofWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            StoredProcedureCall = new StoredProcedureCallRepo(_db);
        }

        public ICategoryRepository Category { get; private set; }

        public IStoredProcedureCall StoredProcedureCall { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
using BulkyBook.DataAccess.Data;
using BulkyBook.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpdateCategory(Category category)
        {
            var objFromDB = _db.Categories.FirstOrDefault(c => c.Id == category.Id);

            if (objFromDB != null)
            {
                _db.Entry<Category>(objFromDB).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                objFromDB.Name = category.Name;
                _db.SaveChanges();
            }
        }
    }
}
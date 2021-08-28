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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var objFromDB = _db.Products.FirstOrDefault(p => p.Id == product.Id);

            if (objFromDB != null)
            {
                if (product.ImageURL != null)
                {
                    objFromDB.ImageURL = product.ImageURL;
                }

                objFromDB.Title = product.Title;
                objFromDB.Author = product.Author;
                objFromDB.CategoryId = product.CategoryId;
                objFromDB.CoverTypeId = product.CoverTypeId;
                objFromDB.Description = product.Description;

                objFromDB.ISBN = product.ISBN;
                objFromDB.ListPrice = product.ListPrice;
                objFromDB.Price = product.Price;
                objFromDB.Price50 = product.Price50;
                objFromDB.Price100 = product.Price100;

                _db.Entry<Product>(objFromDB).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
            }
        }
    }
}
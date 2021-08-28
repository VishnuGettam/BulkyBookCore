using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public interface IUnitofWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IStoredProcedureCall StoredProcedureCall { get; }

        ICoverTypeRepository CoverType { get; }

        IProductRepository Product { get; }

        ICompanyRepository Company { get; }

        IAppUserRepository ApplicationUser { get; }

        void Save();
    }
}
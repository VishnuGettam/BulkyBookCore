using BulkyBook.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public interface IAppUserRepository : IRepository<ApplicationUser>
    {
        // void Update(ApplicationUser applicationUser);
    }
}
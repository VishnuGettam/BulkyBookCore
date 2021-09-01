using BulkyBook.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {
        private readonly IUnitofWork _unitofWork;

        public UserNameViewComponent(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<IViewComponentResult> Invoke()
        {
            var appUser = _unitofWork.ApplicationUser.GetFirstOrDefault(u => u.UserName == User.Identity.Name);

            return await Task.FromResult<IViewComponentResult>(View(appUser));
        }
    }
}
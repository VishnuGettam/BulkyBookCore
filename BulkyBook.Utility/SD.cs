using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public static class SD
    {
        public enum CoverTypeSP
        {
            GetAllCoverType,
            GetCoverType,
            AddCoverType,
            UpdateCoverType,
            DeleteCoverType
        }

        public static string Admin { get { return "Admin"; } set { } }
        public static string Company_Customer { get { return "Company Customer"; } set { } }
        public static string Employee { get { return "Employee"; } set { } }
        public static string Individual_Customer { get { return "Individual Customer"; } set { } }
    }
}
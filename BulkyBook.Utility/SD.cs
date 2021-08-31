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

        public static double GetProductPriceonQuantity(double quantity, double Price, double Price50, double Price100)
        {
            if (quantity < 50)
            {
                return Price;
            }
            else
            {
                if (quantity < 100)
                {
                    return Price50;
                }
                else
                {
                    return Price100;
                }
            }
        }

        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
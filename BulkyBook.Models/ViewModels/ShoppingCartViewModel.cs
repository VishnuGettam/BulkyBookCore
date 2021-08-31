using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public OrderHeader OrderHeader { get; set; }

        public List<ShoppingCart> ListCart { get; set; }
    }
}
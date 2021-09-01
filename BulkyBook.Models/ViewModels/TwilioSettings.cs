using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class TwilioSettings
    {
        public string PhoneNumber { get; set; }
        public string AccountSid { get; set; }

        public string Authtoken { get; set; }
    }
}
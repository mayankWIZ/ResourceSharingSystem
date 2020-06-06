using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class PasswordUpdateModel
    {
        public string username { get; set; }
        public string token { get; set; }
        public string cpass { get; set; }
        public string npass { get; set; }
    }
}
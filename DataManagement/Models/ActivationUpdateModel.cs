using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class ActivationUpdateModel
    {
        public string username { get; set; }
        public string token { get; set; }
        public bool isActive { get; set; }
    }
}
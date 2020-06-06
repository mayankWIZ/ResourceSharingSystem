using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class GroupCreateModel
    {
        public GroupModel group { get; set; }
        public string token { get; set; }
    }
}
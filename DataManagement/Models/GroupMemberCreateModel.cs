using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class GroupMemberCreateModel
    {
        public List<string> users { get; set; }
        public int id { get; set; }
        public string token { get; set; }
    }
}
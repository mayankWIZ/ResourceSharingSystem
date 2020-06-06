using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class TokenUsersModel
    {
        [Key]
        public string token { get; set; }
        public string name { get; set; }
        
    }
}
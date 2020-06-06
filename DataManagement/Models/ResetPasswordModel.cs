using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class ResetPasswordModel
    {
        [Key]
        public string token { get; set; }
        [Required]
        [ForeignKey("user")]
        public string username { set; get; }
        [Required]
        public DateTime gen_time { set; get; }
        public virtual UserModel user { get; set; }
    }
}
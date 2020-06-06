using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class PlanModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        public Double price { get; set; }
        [Required]
        public Double storageBenefit { get; set; }
        [Required]
        public int validity { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public string description { get; set; }
        //public virtual ICollection<User_Plan> user_plans { get; set; }
    }
}
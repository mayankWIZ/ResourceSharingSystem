using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class UserPlanModel
    {
        [Key]
        [ForeignKey("plan")]
        [Column(Order = 1)]
        public int id { get; set; }
        [Key]
        [ForeignKey("user")]
        [Column(Order = 2)]
        public string username { get; set; }
        [Required]
        public DateTime subTime { get; set; }
        [Required]
        public DateTime expiryTime { get; set; }
        [Required]
        public Double storageRemaining { get; set; }
        [Required]
        public int priority { get; set; }

        public virtual UserModel user { get; set; } 
        public virtual PlanModel plan { get; set; }
    }
}
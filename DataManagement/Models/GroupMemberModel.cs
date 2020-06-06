using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class GroupMemberModel
    {
        [Key]
        [ForeignKey("group")]
        [Column(Order = 1)]
        public int groupId { get; set; }
        [Key]
        [ForeignKey("user")]
        [Column(Order = 2)]
        public string username { get; set; }
        [Required]
        public bool reqStatus { get; set; }

        public virtual UserModel user { get; set; }
        public virtual GroupModel group { get; set; }
    }
}
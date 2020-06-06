using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class GroupModel
    {
        [Key]
        public int groupId { get; set; }
        [Required]
        public string groupName { get; set; }
        [Required]
        public int reqPending { get; set; }

        //public virtual ICollection<Group_File_Sharing> group_files { get; set; }
        //public virtual ICollection<Group_Member> group_members { get; set; }
    }
}
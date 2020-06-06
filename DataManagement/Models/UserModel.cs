using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class UserModel
    {
        [Key]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string fname { get; set; }
        [Required]
        public string lname { get; set; }
        [Required]
        public DateTime dob { get; set; }
        [Required]
        public string email { get; set; }
        public DateTime lastloggedin { get; set; }
        public DateTime lastPassChange { get; set; }
        public string usertype { get; set; }
        public bool isActive { get; set; }
        public bool isVerified { get; set; }
        public string profilePic { get; set; }

        //public virtual ICollection<File> files { get; set; }
        //public virtual ICollection<Group_Member> group_members { get; set; }
        //public virtual ICollection<User_Plan> user_plans { get; set; }
        //public virtual Verify_Account verify_account { get; set; }
        //public virtual Reset_Password reset_password { get; set; }
    }
}
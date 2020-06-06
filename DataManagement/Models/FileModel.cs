using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class FileModel
    {
        [Key]
        public string token { get; set; }
        [Required]
        public DateTime sharingDuration { get; set; }
        [Required]
        public string fileName { get; set; }
        [Required]
        public DateTime fileDuration { get; set; }
        [ForeignKey("user")]
        public string username { get; set; }
        [Required]
        public string url { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public double size { get; set; }
        public virtual UserModel user { get; set; }
    }
}
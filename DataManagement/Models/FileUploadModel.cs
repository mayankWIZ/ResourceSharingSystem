using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataManagement.Models
{
    public class FileUploadModel
    {
        public FileModel file { get; set; }
        public string token { get; set; }
    }
}
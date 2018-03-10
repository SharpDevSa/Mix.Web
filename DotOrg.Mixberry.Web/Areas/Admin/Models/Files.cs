using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotOrg.Mixberry.Web.Areas.Admin.Models
{
    public class FileGroups
    {
        public int GroupId { get; set; }
        public string Description { get; set; }
    }

    public class FileImg
    {
        public byte[] content { get; set; }
        public string typeFile { get; set; }
        public string TitleFile { get; set; }
        public string id { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public int order { get; set; }
    }

    public class FileImgContent
    {
        public byte[] content { get; set; }
        public string typeFile { get; set; }
        public string TitleFile { get; set; }
        public FileContentResult fileContent { get; set; }
    }
}
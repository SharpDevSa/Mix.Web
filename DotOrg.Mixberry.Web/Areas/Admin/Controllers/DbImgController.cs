using DotOrg.Mixberry.Web.Areas.Admin.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotOrg.Mixberry.Web.Areas.Admin.Controllers
{
    public class DbImgController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        public void UploadFile(string descr, int id, int oInd)
        {
            descr = DecodeUrlString(descr);
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                DbInter db = new DbInter();
                FileReader fr = new FileReader();
                db.UpLoadFile(hpf.FileName, fr.ReadFully(hpf.InputStream), id, descr, oInd);
            }
        }

        public PartialViewResult EditImg(int id)
        {
            DbInter db = new DbInter();
            var Model = db.DownLoadFileinfo(id);
            return PartialView("EditImg", Model);
        }


        public void SaveImgChange(int Id, string Name, string Descrip, int Order) {
            Descrip = DecodeUrlString(Descrip);
            DbInter db = new DbInter();
            db.UpdateFile(Id, Name, Descrip, Order);
        }

        public PartialViewResult ImgList(int id)
        {
            DbInter db = new DbInter();
            return PartialView("ImgList", db.DownLoadFiles(id));
        }

        public void DeleteFile(int id)
        {
            DbInter db = new DbInter();
            db.DeleteFile(id);

        }

        public JsonResult GetFileGroups()
        {
            DbInter db = new DbInter();
            return Json(db.GetGroups());
        }

        public ActionResult Download(int FileId)
        {
            DbInter dbData = new DbInter();
            var Model = dbData.DownLoadFile(FileId);
            Response.AppendHeader("Content-Disposition", "inline; filename=" + Model.TitleFile);
            if (Model == null || string.IsNullOrEmpty(Model.TitleFile))
                return null;

            if (Model.TitleFile.LastIndexOf('.') != -1)
            {
                if (Model.TitleFile.Substring(Model.TitleFile.LastIndexOf('.')).ToLower() == ".pdf")
                    return File(Model.content, System.Net.Mime.MediaTypeNames.Application.Pdf);
                else if (Model.TitleFile.Substring(Model.TitleFile.LastIndexOf('.')).ToLower() == ".jpg")
                    return File(Model.content, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                else if (Model.TitleFile.Substring(Model.TitleFile.LastIndexOf('.')).ToLower() == ".gif")
                    return File(Model.content, System.Net.Mime.MediaTypeNames.Image.Gif);
                else if (Model.TitleFile.Substring(Model.TitleFile.LastIndexOf('.')).ToLower() == ".txt")
                    return File(Model.content, System.Net.Mime.MediaTypeNames.Text.RichText);
                else
                    return File(Model.content, Model.TitleFile.Substring(Model.TitleFile.LastIndexOf('.')).ToLower());
            }
            else
            {
                return File(Model.content, System.Net.Mime.MediaTypeNames.Application.Zip);
            }

        }
    }
}
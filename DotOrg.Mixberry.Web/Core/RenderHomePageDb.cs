using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotOrg.Mixberry.Models;
using DotOrg.Mixberry.Web.Areas.Admin.Models;
using DotOrg.Mixberry.Web.Areas.Admin.Engine;

namespace DotOrg.Mixberry.Web.Core
{
    public class RenderHomePageDb
    {
        public Location RenderModelHome(Location page, int id)
        {
            DbInter db = new DbInter();
            bool isActive = db.IsActive();

            if (!isActive)
                return page;

            if (page.Alias.ToLower() == "home")
            {
                
                var FileImgL = db.DownLoadFiles(0);
                List<FileImg> TopBannerImgList = null;
                List<FileImg> CategoryImgList = null;
                //English
                if (id == 7)
                {
                    TopBannerImgList = FileImgL.Where(x => x.GroupName.ToLower() == "главная страница (top banner eng)").ToList();
                    CategoryImgList = FileImgL.Where(x => x.GroupName.ToLower() == "главная страница (items eng)").ToList();
                }
                //Russian
                else if (id == 19)
                {
                    TopBannerImgList = FileImgL.Where(x => x.GroupName.ToLower() == "главная страница (top banner ru)").ToList();
                    CategoryImgList = FileImgL.Where(x => x.GroupName.ToLower() == "главная страница (items ru)").ToList();

                }
                if (TopBannerImgList == null)
                    return page;

                foreach (var block in page.Blocks)
                {

                    if (block.Alias == "top-banner")
                    {
                        string StartHeader = "<div id = \"top-page-slider\" class=\"owl-carousel top-page-slider\">";
                        string EndHeader = "</div>";
                        string TopBannerCode = StartHeader;
                        foreach (var MyBlock in TopBannerImgList)
                        {
                            TopBannerCode +=
                            "<div class=\"top-page-slider_item\" style=\"background: url(/DbImg/Download?FileId=" + MyBlock.id + ") no-repeat 50% 50% / cover\">" +
                                "<div class=\"section\">" +
                                    "<div class=\"top-page-slider_item-content\">" +
                                        MyBlock.Description +
                                    "</div>" +
                                "</div>" +
                            "</div>";
                        }
                        TopBannerCode += EndHeader;
                        block.Content = TopBannerCode;
                    }
                    else if (block.Alias == "item-category")
                    {
                        string StartHeader = "<div class=\"home-category-promo section\">";
                        string EndHeader = "</div>";
                        foreach (var myBlock in CategoryImgList)
                        {
                            StartHeader += myBlock.Description;
                        }
                        StartHeader += EndHeader;
                        block.Content = StartHeader;
                    }

                }
            }
            return page;
        }
    }


}
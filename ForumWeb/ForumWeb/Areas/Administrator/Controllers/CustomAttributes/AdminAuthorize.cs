using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumWeb.Models;
namespace ForumWeb.Areas.Administrator.Controllers.CustomAttributes
{
    public class AdminAuthorize : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool isFailed = false;
            string message = "";
            HttpCookie account = filterContext.HttpContext.Request.Cookies["AdminAccount"];
            if (account == null)
            {
                message = "Đăng nhập để tiếp tục!";
                isFailed = true;
            }
            else
            {
                using (var dataContext = new QLDIENDANCONGNGHEDataContext())
                {
                    var id = int.Parse(account["ID"]);
                    var model = dataContext.Ads.Single(a => a.MaAdmin == id);
                    if (model.MatKhau != account["Password"])
                    {
                        message = "Mật khẩu của bạn đã thay đổi, vui lòng đăng nhập lại!";
                        isFailed = true;
                    }
                }

            }
            if (isFailed)
            {
                HttpCookie userCookie = new HttpCookie("AdminAccount");
                userCookie.Expires = DateTime.Now.AddDays(-1);
                filterContext.HttpContext.Response.SetCookie(userCookie);
                filterContext.Controller.TempData["Message"] = message;
                filterContext.Result = new RedirectResult("/Administrator/Account/SignIn");
            }
        }
    }
}
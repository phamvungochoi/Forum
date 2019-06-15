using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumWeb.Models;
using ForumWeb.Areas.Administrator.Models;
namespace ForumWeb.Areas.Administrator.Controllers
{
    public class AccountController : Controller
    {
        private QLDIENDANCONGNGHEDataContext db = new QLDIENDANCONGNGHEDataContext();
        // GET: Administrator/Account
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost, ActionName("SignIn")]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AdminViewModel account, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var isValid = db.Ads.SingleOrDefault(p => p.TenDangNhap == account.TenDangNhap);
                if (isValid == null)
                {
                    ViewBag.Message = "Tài khoản không tồn tại.";
                    return View(account);
                }
                else if (isValid.MatKhau != account.MatKhau)
                {
                    ViewBag.Message = "Mật khẩu bị sai.";
                    return View(account);
                }
                HttpCookie userCookie = new HttpCookie("AdminAccount", account.TenDangNhap);
                userCookie["ID"] = isValid.MaAdmin.ToString();
                userCookie["Username"] = account.TenDangNhap;
                userCookie["Password"] = account.MatKhau;
                userCookie.Expires = DateTime.Now.AddDays(1);
                Response.SetCookie(userCookie);
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(account);
        }
        public ActionResult SignOut()
        {
            HttpCookie userCookie = new HttpCookie("AdminAccount");
            userCookie.Expires = DateTime.Now.AddDays(-1);
            Response.SetCookie(userCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}
using ForumWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace ForumWeb.Controllers
{
    public class ForumController : Controller
    {
        QLDIENDANCONGNGHEDataContext data = new QLDIENDANCONGNGHEDataContext();
        // GET: Forum
        private List<BaiGui> GetBaiGuis(int count)
        {
            return data.BaiGuis.OrderByDescending(a => a.NgayGuiBai).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            var baiguimoi = GetBaiGuis(8);
            return View(baiguimoi.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult RecentPost()
        {
            var baiguimoi = GetBaiGuis(3);
            return PartialView(baiguimoi);
        }
        public ActionResult LinhVuc()
        {
            var linhvuc = from lv in data.LinhVucs select lv;
            return PartialView(linhvuc);
        }
        public ActionResult TheoLinhVuc(int id, int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            var baigui = from bg in data.BaiGuis where bg.MaLinhVuc == id select bg;
            return View(baigui.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult CongDong()
        {
            var congdong = from cd in data.CongDongs select cd;
            return PartialView(congdong);
        }
        public ActionResult TheoCongDong(int id, int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            var baigui = from bg in data.BaiGuis where bg.MaBaiGui == id select bg;
            return View(baigui.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChuDe()
        {
            var chude = from cde in data.ChuDes select cde;
            return PartialView(chude);
        }
        public ActionResult TheoChuDe(int id, int ? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            var baigui = from bg in data.BaiGuis where bg.MaChuDe == id select bg;
            return View(baigui.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChiTietBaiGui(int id)
        {
            var baigui = from bg in data.BaiGuis where bg.MaBaiGui == id select bg;
            return View(baigui.Single());
        }
        [HttpGet]
        public ActionResult VietBai()
        {
            if (Session["TenDangNhap"] == null)
            {
                return RedirectToAction("Dangnhap", "NguoiDung");
            }
            ViewBag.MaChuDe = new SelectList(data.ChuDes, "MaChuDe", "TenChuDe");
            ViewBag.MaCongDong = new SelectList(data.CongDongs, "MaCongDong", "TenCongDong");
            ViewBag.MaLinhVuc = new SelectList(data.LinhVucs, "MaLinhVuc", "TenLinhVuc");
            return View();
        }
        [HttpPost]
        public ActionResult VietBai(FormCollection collection)
        {
            var baiViet = new BaiGui
            {
                MaChuDe = int.Parse(collection["MaChuDe"]),
                MaLinhVuc = int.Parse(collection["MaLinhVuc"]),
                MaCongDong = int.Parse(collection["MaCongDong"]),
                SoDienThoai = collection["SoDienThoai"].ToString().Trim(),
                Email = collection["Email"].ToString().Trim(),
                NgayGuiBai = DateTime.Now,
                TenBai = collection["TenBai"].ToString().Trim(),
                NoiDung = collection["NoiDung"].ToString().Trim()
            };
            var message = "";
            if (!Regex.IsMatch(baiViet.SoDienThoai,@"^\d{10}$"))
            {
                message += "Số điện thoại không được bỏ trống";
            }
            if (string.IsNullOrWhiteSpace(baiViet.TenBai))
            {
                message += "Tên bài không được bỏ trống";
            }
            if(!string.IsNullOrEmpty(message))
            {
                ViewBag.MaChuDe = new SelectList(data.ChuDes, "MaChuDe", "TenChuDe");
                ViewBag.MaCongDong = new SelectList(data.CongDongs, "MaCongDong", "TenCongDong");
                ViewBag.MaLinhVuc = new SelectList(data.LinhVucs, "MaLinhVuc", "TenLinhVuc");
                return View("VietBai",baiViet);
            }
            baiViet.MaNguoiSuDung = (Session["TenDangNhap"] as NguoiSuDung).MaNguoiSuDung;
            data.BaiGuis.InsertOnSubmit(baiViet);
            data.SubmitChanges();
            return RedirectToAction("Index", "Forum");
        }
    }
}
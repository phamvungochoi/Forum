using ForumWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumWeb.Controllers
{
    public class NguoiDungController : Controller
    {
        QLDIENDANCONGNGHEDataContext data = new QLDIENDANCONGNGHEDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, NguoiSuDung NSD)
        {
            var TenDangNhap = collection["TenDangNhap"];
            var MatKhau = collection["MatKhau"];
            var MatKhauNhapLai = collection["MatKhauNhapLai"];
            var HoTen = collection["HoTen"];
            var DiaChi = collection["DiaChi"];
            var GioiTinh = collection["GioiTinh"];
            var NgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            var SoDienThoai = collection["SoDienThoai"];
            var Email = collection["Email"];
            if (String.IsNullOrEmpty(TenDangNhap))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(MatKhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(MatKhauNhapLai))
            {
                ViewData["Loi3"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(HoTen))
            {
                ViewData["Loi4"] = "Phải nhập họ tên";
            }
            //else if (String.IsNullOrEmpty(DiaChi))
            //{
            //    ViewData["Loi5"] = "Phải nhập địa chỉ";
            //}
            else if (String.IsNullOrEmpty(GioiTinh))
            {
                ViewData["Loi6"] = "Phải nhập giới tính";
            }
            else if (String.IsNullOrEmpty(SoDienThoai))
            {
                ViewData["Loi7"] = "Phải nhập số điện thoại";
            }
            if (String.IsNullOrEmpty(Email))
            {
                ViewData["Loi8"] = "Phải nhập email";
            }
            else
            {
                NSD.TenDangNhap = TenDangNhap;
                NSD.MatKhau = MatKhau;
                NSD.HoTen = HoTen;
                NSD.DiaChi = DiaChi;
                NSD.GioiTinh = GioiTinh;
                NSD.NgaySinh = DateTime.Parse(NgaySinh);
                NSD.SoDienThoai = SoDienThoai;
                NSD.Email = Email;
                data.NguoiSuDungs.InsertOnSubmit(NSD);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var TenDangNhap = collection["TenDangNhap"];
            var MatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(TenDangNhap))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(MatKhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                NguoiSuDung NSD = data.NguoiSuDungs.SingleOrDefault(n => n.TenDangNhap == TenDangNhap && n.MatKhau == MatKhau);
                if (NSD != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TenDangNhap"] = NSD;
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return RedirectToAction("Index", "Forum");
        }
        public PartialViewResult ID()
        {
            if (Session["TenDangNhap"] != null)
            {
                NguoiSuDung nsd = (NguoiSuDung) Session["TenDangNhap"];
                ViewBag.ThongBao =nsd.TenDangNhap;
            }
            return PartialView();
        }
    }
}
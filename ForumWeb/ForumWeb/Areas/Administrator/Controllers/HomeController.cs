using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using ForumWeb.Models;
using ForumWeb.Areas.Administrator.Controllers.CustomAttributes;

namespace ForumWeb.Areas.Administrator.Controllers
{
    public class HomeController : Controller
    {
        QLDIENDANCONGNGHEDataContext db = new QLDIENDANCONGNGHEDataContext();
        // GET: Administrator/Home
        public ActionResult Index()
        {
            return View();
        }
        [AdminAuthorize]
        public ActionResult BaiViet(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.BaiGuis.ToList().OrderBy(n => n.MaBaiGui).ToPagedList(pageNumber, pageSize));
        }
        [AdminAuthorize]
        public ActionResult Details(int? id)
        {
            BaiGui baigui= db.BaiGuis.SingleOrDefault(n => n.MaBaiGui == id);
            ViewBag.MaBaiGui = baigui.MaBaiGui;
            if (baigui == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(baigui);
        }
        [AdminAuthorize]
        public ActionResult XoaBai(int? id)
        {
            BaiGui baigui = db.BaiGuis.SingleOrDefault(n => n.MaBaiGui == id);
            ViewBag.MaBaiGui = baigui.MaBaiGui;
            if (baigui == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(baigui);
        }
        [HttpPost, ActionName("XoaBai")]
        public ActionResult XacNhanXoa(int? id)
        {
            BaiGui baigui = db.BaiGuis.SingleOrDefault(n => n.MaBaiGui == id);
            ViewBag.MaBaiGui = baigui.MaBaiGui;
            if (baigui == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.BaiGuis.DeleteOnSubmit(baigui);
            db.SubmitChanges();
            return RedirectToAction("BaiViet");
        }
        [AdminAuthorize]
        public ActionResult SuaBai(int? id)
        {
            BaiGui baigui = db.BaiGuis.SingleOrDefault(n => n.MaBaiGui == id);
            ViewBag.MaBaiGui = baigui.MaBaiGui;
            if (baigui == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaChuDe = new SelectList(db.ChuDes, "MaChuDe", "TenChuDe");
            ViewBag.MaCongDong = new SelectList(db.CongDongs, "MaCongDong", "TenCongDong");
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc");
            return View(baigui);
        }
        [AdminAuthorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaBai(BaiGui bg)
        {
            if (ModelState.IsValid)
            {
                var obj = db.BaiGuis.SingleOrDefault(p => p.MaBaiGui == bg.MaBaiGui);
                obj.MaChuDe = bg.MaChuDe;
                obj.MaCongDong = bg.MaCongDong;
                obj.MaLinhVuc = bg.MaLinhVuc;
                obj.MaNguoiSuDung = bg.MaNguoiSuDung;
                obj.SoDienThoai = bg.SoDienThoai;
                obj.Email = bg.Email;
                obj.NgayGuiBai = bg.NgayGuiBai;
                obj.TenBai = bg.TenBai;
                obj.NoiDung = bg.NoiDung;
                db.SubmitChanges();
                return RedirectToAction("BaiViet");
            }
            ViewBag.MaChuDe = new SelectList(db.ChuDes, "MaChuDe", "TenChuDe");
            ViewBag.MaCongDong = new SelectList(db.CongDongs, "MaCongDong", "TenCongDong");
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs, "MaLinhVuc", "TenLinhVuc");
            return View(bg);
        }
        [AdminAuthorize]
        public ActionResult QuanLy(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Ads.ToList().OrderBy(n => n.MaAdmin).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThemMoiAd()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiAd(Ad ad)
        {
            if (ModelState.IsValid)
            {
                db.Ads.InsertOnSubmit(ad);
                db.SubmitChanges();
            }
            return RedirectToAction("QuanLy", "Home");
        }
        [AdminAuthorize]
        public ActionResult XoaAd(int? id)
        {
            Ad ad = db.Ads.SingleOrDefault(n => n.MaAdmin == id);
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ad);
        }
        [HttpPost, ActionName("XoaAd")]
        public ActionResult XacNhanXoaAd(int? id)
        {
            Ad ad = db.Ads.SingleOrDefault(n => n.MaAdmin == id);
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Ads.DeleteOnSubmit(ad);
            db.SubmitChanges();
            return RedirectToAction("QuanLy");
        }
        [AdminAuthorize]
        public ActionResult ChuDe(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.ChuDes.ToList().OrderBy(n => n.MaChuDe).ToPagedList(pageNumber, pageSize));
        }
        [AdminAuthorize]
        public ActionResult ThemMoiCD()
        {
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiCD(ChuDe chude)
        {
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            if (ModelState.IsValid)
            {
                db.ChuDes.InsertOnSubmit(chude);
                db.SubmitChanges();
            }
            return RedirectToAction("ChuDe", "Home");
        }
        [AdminAuthorize]
        public ActionResult XoaCD(int? id)
        {
            ChuDe chude = db.ChuDes.SingleOrDefault(n => n.MaChuDe == id);
            ViewBag.MaChuDe = chude.MaChuDe;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chude);
        }
        [AdminAuthorize]
        [HttpPost, ActionName("XoaCD")]
        public ActionResult XacNhanXoaCD(int? id)
        {
            ChuDe chude = db.ChuDes.SingleOrDefault(n => n.MaChuDe == id);
            ViewBag.MaChuDe = chude.MaChuDe;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.ChuDes.DeleteOnSubmit(chude);
            db.SubmitChanges();
            return RedirectToAction("ChuDe");
        }
        [AdminAuthorize]
        public ActionResult SuaCD(int? id)
        {
            ChuDe chude = db.ChuDes.SingleOrDefault(n => n.MaChuDe == id);
            ViewBag.MaChuDe = chude.MaChuDe;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe", chude.MaChuDe);
            return View(chude);
        }
        [AdminAuthorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaCD(ChuDe chude)
        {
            if (ModelState.IsValid)
            {

                var obj = db.ChuDes.SingleOrDefault(p => p.MaChuDe == chude.MaChuDe);
                obj.TenChuDe = chude.TenChuDe;
                db.SubmitChanges();
                return RedirectToAction("ChuDe");
            }
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            return View(chude);
        }

        [AdminAuthorize]
        public ActionResult CongDong(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.CongDongs.ToList().OrderBy(n => n.MaCongDong).ToPagedList(pageNumber, pageSize));
        }
        [AdminAuthorize]
        public ActionResult ThemMoiCongDong()
        {
            ViewBag.MaCongDong = new SelectList(db.CongDongs.ToList().OrderBy(n => n.TenCongDong), "MaCongDong", "TenCongDong");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiCongDong(CongDong congdong)
        {
            ViewBag.MaCongDong = new SelectList(db.CongDongs.ToList().OrderBy(n => n.TenCongDong), "MaCongDong", "TenCongDong");
            if (ModelState.IsValid)
            {
                db.CongDongs.InsertOnSubmit(congdong);
                db.SubmitChanges();
            }
            return RedirectToAction("CongDong", "Home");
        }
        [AdminAuthorize]
        public ActionResult XoaCongDong(int? id)
        {
            CongDong congdong = db.CongDongs.SingleOrDefault(n => n.MaCongDong == id);
            ViewBag.MaCongDong = congdong.MaCongDong;
            if (congdong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(congdong);
        }
        [AdminAuthorize]
        [HttpPost, ActionName("XoaCongDong")]
        public ActionResult XacNhanXoaCongDong(int? id)
        {
            CongDong congdong = db.CongDongs.SingleOrDefault(n => n.MaCongDong == id);
            ViewBag.MaCongDong = congdong.MaCongDong;
            if (congdong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.CongDongs.DeleteOnSubmit(congdong);
            db.SubmitChanges();
            return RedirectToAction("CongDong");
        }
        [AdminAuthorize]
        public ActionResult SuaCongDong(int? id)
        {
            CongDong congdong = db.CongDongs.SingleOrDefault(n => n.MaCongDong == id);
            ViewBag.MaCongDong = congdong.MaCongDong;
            if (congdong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCongDong = new SelectList(db.CongDongs.ToList().OrderBy(n => n.TenCongDong), "MaCongDong", "TenCongDong", congdong.MaCongDong);
            return View(congdong);
        }
        [AdminAuthorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaCongDong(CongDong congdong)
        {
            if (ModelState.IsValid)
            {

                var obj = db.CongDongs.SingleOrDefault(p => p.MaCongDong == congdong.MaCongDong);
                obj.TenCongDong = congdong.TenCongDong;
                db.SubmitChanges();
                return RedirectToAction("CongDong");
            }
            ViewBag.MaCongDong = new SelectList(db.CongDongs.ToList().OrderBy(n => n.TenCongDong), "MaCongDong", "TenCongDong");
            return View(congdong);
        }

        [AdminAuthorize]
        public ActionResult LinhVuc(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.LinhVucs.ToList().OrderBy(n => n.MaLinhVuc).ToPagedList(pageNumber, pageSize));
        }
        [AdminAuthorize]
        public ActionResult ThemMoiLV()
        {
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs.ToList().OrderBy(n => n.TenLinhVuc), "MaLinhVuc", "TenLinhVuc");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiLV(LinhVuc linhvuc)
        {
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs.ToList().OrderBy(n => n.TenLinhVuc), "MaLinhVuc", "TenLinhVuc");
            if (ModelState.IsValid)
            {
                db.LinhVucs.InsertOnSubmit(linhvuc);
                db.SubmitChanges();
            }
            return RedirectToAction("LinhVuc", "Home");
        }
        [AdminAuthorize]
        public ActionResult XoaLV(int? id)
        {
            LinhVuc linhvuc = db.LinhVucs.SingleOrDefault(n => n.MaLinhVuc == id);
            ViewBag.MaLinhVuc = linhvuc.MaLinhVuc;
            if (linhvuc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(linhvuc);
        }
        [AdminAuthorize]
        [HttpPost, ActionName("XoaLV")]
        public ActionResult XacNhanXoaLV(int? id)
        {
            LinhVuc linhvuc = db.LinhVucs.SingleOrDefault(n => n.MaLinhVuc == id);
            ViewBag.MaLinhVuc = linhvuc.MaLinhVuc;
            if (linhvuc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LinhVucs.DeleteOnSubmit(linhvuc);
            db.SubmitChanges();
            return RedirectToAction("LinhVuc");
        }
        [AdminAuthorize]
        public ActionResult SuaLV(int? id)
        {
            LinhVuc linhvuc = db.LinhVucs.SingleOrDefault(n => n.MaLinhVuc == id);
            ViewBag.MaLinhVuc = linhvuc.MaLinhVuc;
            if (linhvuc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs.ToList().OrderBy(n => n.TenLinhVuc), "MaLinhVuc", "TenLinhVuc", linhvuc.MaLinhVuc);
            return View(linhvuc);
        }
        [AdminAuthorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaLV(LinhVuc linhvuc)
        {
            if (ModelState.IsValid)
            {

                var obj = db.LinhVucs.SingleOrDefault(p => p.MaLinhVuc == linhvuc.MaLinhVuc);
                obj.TenLinhVuc = linhvuc.TenLinhVuc;
                db.SubmitChanges();
                return RedirectToAction("LinhVuc");
            }
            ViewBag.MaLinhVuc = new SelectList(db.LinhVucs.ToList().OrderBy(n => n.TenLinhVuc), "MaLinhVuc", "TenLinhVuc");
            return View(linhvuc);
        }
        [AdminAuthorize]
        public ActionResult NguoiSuDung(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.NguoiSuDungs.ToList().OrderBy(n => n.MaNguoiSuDung).ToPagedList(pageNumber, pageSize));
        }
       
        [AdminAuthorize]
        public ActionResult ChiTietNSD(int? id)
        {
            NguoiSuDung nsd = db.NguoiSuDungs.SingleOrDefault(n => n.MaNguoiSuDung == id);

            if (nsd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nsd);
        }
        [AdminAuthorize]
        public ActionResult XoaNSD(int? id)
        {
            NguoiSuDung nsd = db.NguoiSuDungs.SingleOrDefault(n => n.MaNguoiSuDung == id);
            if (nsd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nsd);
        }
        [HttpPost, ActionName("XoaNSD")]
        public ActionResult XacNhanXoaNV(int? id)
        {
            NguoiSuDung nsd = db.NguoiSuDungs.SingleOrDefault(n => n.MaNguoiSuDung == id);
            if (nsd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NguoiSuDungs.DeleteOnSubmit(nsd);
            db.SubmitChanges();
            return RedirectToAction("NguoiSuDung");
        }
        [AdminAuthorize]
        public ActionResult SuaNSD(int? id)
        {
            NguoiSuDung nsd = db.NguoiSuDungs.SingleOrDefault(n => n.MaNguoiSuDung == id);
            if (nsd == null)
            {
                Response.StatusCode = 404;
                return null;
            }           
            return View(nsd);
        }
        [AdminAuthorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaNSD([Bind(Include = "MaNguoiSuDung, TenDangNhap, MatKhau, HoTen, DiaChi,  GioiTinh, NgaySinh, SoDienThoai, Email")] NguoiSuDung nsd)
        {
            if (ModelState.IsValid)
            {
                var obj = db.NguoiSuDungs.SingleOrDefault(p => p.MaNguoiSuDung == nsd.MaNguoiSuDung);
                obj.TenDangNhap = nsd.TenDangNhap;
                obj.MatKhau = nsd.MatKhau;
                obj.HoTen = nsd.HoTen;
                obj.DiaChi = nsd.DiaChi;
                 obj.GioiTinh = nsd.GioiTinh;
                obj.NgaySinh = nsd.NgaySinh;
                obj.SoDienThoai = nsd.SoDienThoai;
                obj.Email = nsd.Email;
                db.SubmitChanges();
                return RedirectToAction("NguoiSuDung");
            };
            return View(nsd);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebXeGanMay.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.UI.WebControls;
using System.Data.Entity.Migrations;
namespace WebXeGanMay.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        QLBanXeGanMayEntities db = new QLBanXeGanMayEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult XeMay(int ?page)
        {
            int pageSize = 7;
            int pageNum = (page ?? 1);
            var list = db.XEGANMAYs.OrderBy(x => x.MaXe).ToList();
            return View(list.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ThemMoi()
        {
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n=>n.TenLoaiXe),"MaLX","TenLoaiXe");
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(XEGANMAY xemay , HttpPostedFileBase fileUpload)
        {
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            if(fileUpload == null)
            {
                ViewBag.ThongBao = "Vui Lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/content/Images/Xe"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    xemay.Anhbia = fileName;
                    db.XEGANMAYs.Add(xemay);
                    db.SaveChanges();
                   
                }
                return RedirectToAction("XeMay","Admin");
            }
            
        }
        
        public ActionResult ChiTietSanPham(int id)
        {
            var xemay = db.XEGANMAYs.SingleOrDefault(c=>c.MaXe == id);
            if(xemay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xemay);
        }

        public ActionResult Xoa(int id)
        {
            var xemay = db.XEGANMAYs.SingleOrDefault(db=>db.MaXe == id);
            if (xemay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xemay);
        }
        [HttpPost,ActionName("Xoa")]
        public ActionResult xacNhanXoa(int id)
        {
            var xemay = db.XEGANMAYs.SingleOrDefault(c => c.MaXe == id);
            if (xemay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.XEGANMAYs.Remove(xemay);
            db.SaveChanges();
            return RedirectToAction("XeMay","Admin");

        }
        [HttpGet]
        public ActionResult SuaXeMay(int id)
        {
            XEGANMAY xemay = db.XEGANMAYs.SingleOrDefault(c => c.MaXe == id);
            if (xemay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe",xemay.MaLX);
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP" ,xemay.NHAPHANPHOI);
            return View(xemay);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaXeMay(XEGANMAY xemay, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(db.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui Lòng chọn ảnh bìa";
                return View(xemay);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/content/Images/Xe"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    xemay.Anhbia = fileName;
                    db.XEGANMAYs.AddOrUpdate(xemay);
                    db.SaveChanges();

                }
                return RedirectToAction("XeMay", "Admin");
            }
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(FormCollection collection)
        {
            var userName = collection["UserName"];
            var password = collection["password"];
            if (string.IsNullOrEmpty(userName))
            {
                ViewData["loi1"] = "Tên đăng nhập không được để trống";
            }
            else if (string.IsNullOrEmpty(password))
            {
                ViewData["loi2"] = "Mật khẩu không được để trống";
            }

            else
            {
                Admin admin = db.Admins.Where(a=> a.UserName == userName && a.PassAdmin == password).FirstOrDefault();
                if (admin != null)
                {
                    Session["IdAdmin"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        public ActionResult thongKe()
        {
            
                var thongKeData = db.XEGANMAYs
                    .GroupBy(x => x.LOAIXE.TenLoaiXe)
                    .Select(g => new
                    {
                        TenLoaiXe = g.Key,       
                        SoLuong = g.Sum(x => x.Soluongton ?? 0) 
                    })
                    .ToList();

                
                ViewBag.ThongKeData = thongKeData;

                return View();
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebXeGanMay.Models;

namespace WebXeGanMay.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        QLBanXeGanMayEntities db =  new QLBanXeGanMayEntities();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection , KHACHHANG kh)
        {
            var hoten = collection["txthoten"].ToString();
            var ngaySinh =String.Format("{0:MM/dd/yyyy}", collection["txtngaysinh"]);
            var diaChi = collection["txtdiachi"].ToString();
            var sodt = collection["soDienThoai"].ToString();
            var email = collection["txtemail"].ToString() ;
            var tenDangNhap = collection["txttendangnhap"].ToString();
            var matKhau = collection["txtmatkhau"].ToString();
            var matkhaunhaplai = collection["txttendangnhap"].ToString();
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(ngaySinh))
            {
                ViewData["Loi2"] = "Ngày sinh khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(diaChi))
            {
                ViewData["Loi3"] = "Địa chỉ khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(sodt))
            {
                ViewData["Loi4"] = "Số điện thoại khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(tenDangNhap))
            {
                ViewData["Loi6"] = "Tên đăng nhập khách hàng không được để trống";

            }
            if (String.IsNullOrEmpty(matKhau))
            {
                ViewData["Loi7"] = "Mật khẩu khách hàng không được để trống";

            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi8"] = "Mật khẩu nhập lại  khách hàng không được để trống";
                if(matKhau != matkhaunhaplai)
                {
                    ViewData["Loi8"] = "Mật khẩu nhập lại đã sai";
                }
            }
            else
            {
                kh.HoTen = hoten;
                kh.Ngaysinh = DateTime.Parse( ngaySinh);
                kh.Email = email;
                kh.DienthoaiKH = sodt;
                kh.DiachiKH = diaChi;
                kh.Taikhoan = tenDangNhap;
                kh.Matkhau = matKhau;
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection con)
        {
            var tenDangNhap = con["hoten"].ToString();
            var mk = con["mk"].ToString();
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                ViewData["loi1"] = "Tên đăng nhập không được để trống";
            }
            else if (string.IsNullOrEmpty(mk))
            {
                ViewData["loi1"] = "Mật khẩu không được để trống";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(c => c.Taikhoan == tenDangNhap && c.Matkhau == mk);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công";
                    Session["customer"] = kh;
                    return RedirectToAction("index", "Store");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";

                }
                
            }
            return View();
        }
    }
}
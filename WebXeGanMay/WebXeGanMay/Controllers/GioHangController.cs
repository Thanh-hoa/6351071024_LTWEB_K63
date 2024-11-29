using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebXeGanMay.Models;

namespace WebXeGanMay.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioiHang
        QLBanXeGanMayEntities db = new QLBanXeGanMayEntities();
        public List<GioHang> layGioHang()
        {
            List<GioHang>listGioHang = Session["GioHang"] as List<GioHang>;
            if(listGioHang == null)
            {
                listGioHang = new List<GioHang>();
                Session["GioHang"] = listGioHang;
            }
            return listGioHang;
        }
        public ActionResult themGioHang(int id , string url)
        {
            List<GioHang> listGioHang = layGioHang();
            GioHang sanPham = listGioHang.Find(s => s.smaXe == id);
            if(sanPham == null)
            {
                sanPham = new GioHang(id);
                listGioHang.Add(sanPham);
               
            }
            else
            {
                sanPham.soLuong++;
             
            }
            return RedirectToAction
                ("Index" ,
                "Store");
        }
        public int tongSoLuong()
        {
            int IsTongSoLuong = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                IsTongSoLuong = listGiohang.Sum(n => n.soLuong);
            }
            return IsTongSoLuong;

        }
        public double TongTien()
        {
            double IsTongTien = 0;
            List<GioHang> ListGioHang = Session["GioHang"] as List<GioHang>;
            if(ListGioHang != null)
            {
                IsTongTien = ListGioHang.Sum(s => s.dThanhTien);
            }
            return IsTongTien;
        }
        public ActionResult Index()
        {
            List<GioHang> ListGioHang = layGioHang();
            if(ListGioHang.Count < 0)
            {
                return RedirectToAction("Index","Store");
            }
            ViewData["TongSoLuong"] = tongSoLuong();
            ViewData["TongTien"] = TongTien();
            return View(ListGioHang);
        }
        public ActionResult GioHangPartVial()
        {
            ViewData["TongSoLuong"] = tongSoLuong();
            ViewData["TongTien"] = TongTien();
            return PartialView();
        }
        public ActionResult Xoa(int id)
        {
            List<GioHang> ListGioHang = layGioHang();
            var sanpham = ListGioHang.SingleOrDefault(c => c.smaXe == id);
            if (sanpham != null) {
                ListGioHang.Remove(sanpham);
                return RedirectToAction("Index","GioHang");
            }
            if(ListGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Store");
            }
            return RedirectToAction("Index","GioHang");
        }
        public ActionResult CapNhat(int id , FormCollection con)
        {
            List<GioHang> listGioHang = layGioHang();
            GioHang sanPham = listGioHang.SingleOrDefault(n => n.smaXe == id);
            if (sanPham != null) {
                sanPham.soLuong = int.Parse(con["txtSoluong"].ToString()); 
            }
            return RedirectToAction("Index","GioHang");
        }
        public ActionResult XoaTatCa()
        {
            List<GioHang> listGioHang = layGioHang();
            listGioHang.Clear();
           
            return RedirectToAction("Index", "Store");
        }
        [HttpGet]
        public ActionResult DonHang()
        {
            if(Session["customer"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Store");
            }
            List<GioHang> ListGioHang = layGioHang();
            ViewData["TongSoLuong"] = tongSoLuong();
            ViewData["TongTien"] = TongTien();
            return View(ListGioHang);
        }
        [HttpPost]
        public ActionResult DonHang(FormCollection collection)
        {
            // Thêm Đơn hàng
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["customer"];
            List<GioHang> gh = layGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();

            // Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaXe = item.smaXe;
                ctdh.Soluong = item.soLuong;
                ctdh.Dongia = (decimal)item.dThanhTien;
                db.CHITIETDONTHANGs.Add(ctdh);
            }
            db.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }

    }
}
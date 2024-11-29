﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebXeGanMay.Models;

namespace WebXeGanMay.Controllers
{

    public class GioHang
    {
        QLBanXeGanMayEntities db = new QLBanXeGanMayEntities();
        public int smaXe { get; set; }
        public string stenXe { get; set; }
        public string sAnhBia { get; set; }
        public Double sdonGia { get; set; }
        public int soLuong { get; set; }
        public Double dThanhTien
        {
            get { return sdonGia * soLuong; }
        }
        public GioHang(int maXe)
        {
            smaXe = maXe;
            XEGANMAY xe = db.XEGANMAYs.Single(x=> x.MaXe == smaXe);
            stenXe = xe.TenXe;
            sAnhBia = xe.Anhbia;
            sdonGia = double.Parse(xe.Giaban.ToString());
            soLuong = 1;

        }
    }
}
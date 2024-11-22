using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebXeGanMay.Models;

namespace WebXeGanMay.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        QLBanXeGanMayEntities db = new QLBanXeGanMayEntities();
        public ActionResult Index()
        {
            var listXe = db.XEGANMAYs.Take(5).ToList();
            return View(listXe);
        }
        public ActionResult Detail(int id) 
        {
            var product = db.XEGANMAYs.FirstOrDefault(c => c.MaXe == id);
            return View(product);
        }
        public ActionResult XeHonDa()
        {
            var ListHonDa = db.XEGANMAYs.Where(c => c.MaLX == 1).ToList();
            return View(ListHonDa);
        }
        public ActionResult XeYAMAHA()
        {
            var ListHonDa = db.XEGANMAYs.Where(c => c.MaLX == 2).ToList();
            return View(ListHonDa);
        }
        public ActionResult XeSUZUKI()
        {
            var ListHonDa = db.XEGANMAYs.Where(c => c.MaLX == 3).ToList();
            return View(ListHonDa);
        }
        public ActionResult XeSYM()
        {
            var ListHonDa = db.XEGANMAYs.Where(c => c.MaLX == 4).ToList();
            return View(ListHonDa);

        }
        public ActionResult XePIAGGIO()
        {
            var ListHonDa = db.XEGANMAYs.Where(c => c.MaLX == 5).ToList();
            return View(ListHonDa);

        }
    }
}
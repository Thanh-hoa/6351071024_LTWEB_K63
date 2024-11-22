using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebXeGanMay.Models;

namespace WebXeGanMay.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
       QLBanXeGanMayEntities db = new QLBanXeGanMayEntities();
        //public ActionResult Index()
        //{
        //    var ListNPP = db.NHAPHANPHOIs.Take(5).ToList();
        //    return PartialView(ListNPP);
        //}
        public ActionResult Index()
        {
            var ListNPP = db.XEGANMAYs.Take(5).ToList();
            return View(ListNPP);
        }

        public ActionResult Menu()
        {
            var ListNPP = db.LOAIXEs.ToList();
            return PartialView(ListNPP);
        }

        public ActionResult NPP( )
        {
            var ListNPP = db.NHAPHANPHOIs.ToList();
            return PartialView(ListNPP);
        }
        public ActionResult SPTTheoMenu(int id)
        {
            var ListLoaiXe = db.XEGANMAYs.Where(c=> c.MaLX==id).ToList();
            return View(ListLoaiXe);
        }

        public ActionResult SPTNhaPP(int id)
        {
            var ListLoaiXe = db.XEGANMAYs.Where(c => c.MaNPP == id).ToList();
            return View(ListLoaiXe);
        }
        public ActionResult Detail(int id)
        {
            var product = db.XEGANMAYs.FirstOrDefault(c => c.MaXe == id);
            return View(product);
        }

    }
}
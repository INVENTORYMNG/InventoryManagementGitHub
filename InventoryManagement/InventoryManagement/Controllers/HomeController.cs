using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        Models.INVENTORYMNGDBEntities INVENTORYMNGDB = new Models.INVENTORYMNGDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _IndexMenuPV()
        {
            try
            {
                ViewBag.GetMenu = INVENTORYMNGDB.sp_GetMenuByRole(1, 1, 0).ToList();
            }
            catch { }

            return PartialView();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
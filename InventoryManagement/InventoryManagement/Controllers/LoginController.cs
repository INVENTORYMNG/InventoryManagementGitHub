using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagement.Controllers
{
    public class LoginController : Controller
    {
        INVENTORYMNGDBEntities InventoryDB = new INVENTORYMNGDBEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            //Session["changePassword"] = null;
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Now);
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string userName, string password)
        {
            int? userID = 0;
            try
            {
                if (userName == null && password == null)
                {
                    return View("SignIn");
                }
                else
                {
                    userID = InventoryDB.spInventoryGetUserID(userName, password).FirstOrDefault();
                    if (userID > 0)
                    {
                        Session["userID"] = userID;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ErrorViewBagMSJ("Please contact support");
                        return View("SignIn");
                    }
                }
            }
            catch (Exception error)
            {
                ErrorViewBagMSJ("Please contact support");
                return View("SignIn");
            }
            //return View("SignIn");
        }
        private string ErrorViewBagMSJ(string msj)
        {
            ViewBag.Message = msj;
            ViewBag.Type = "1"; //Error
            return ViewBag.Message;
        }
    }
}
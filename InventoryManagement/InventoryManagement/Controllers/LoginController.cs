using Antlr.Runtime.Tree;
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
                        ErrorViewBagMSJ("User name or Password is incorrect!");
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

        public ActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpPost]
        public ActionResult ChangePassword(string token, string newPassword, string newConfirmPassword)
        {
            try
            {
                if (token != null)
                {
                    int userByTokenID = 0;
                    userByTokenID = InventoryDB.spInventoryGetUserIDByToken(int.Parse(token)).FirstOrDefault().Value;
                    if (userByTokenID > 0)
                    {
                        if (newPassword == newConfirmPassword)
                        {
                            try
                            {
                                int resp;
                                InventoryDB.spUserSetNewPassword(userByTokenID,newPassword);
                                SuccesViewBagMSJ("the password was changed, please login again with your new password");
                                return View("SignIn");
                            }
                            catch
                            {
                                ErrorViewBagMSJ("Please contact support!");
                                return View("ChangePassword");
                            }
                        }
                        else
                        {
                            InfoResultViewBagMSJ("Passwords do not match");
                            return View("ChangePassword");
                        }
                    }
                    else
                    {
                        ErrorViewBagMSJ("Invalid Token!");
                        return View("ChangePassword");
                    }
                }
                else
                {
                    ErrorViewBagMSJ("Invalid Token!");
                    return View("ChangePassword");
                }
            }
            catch(Exception ex)
            {
                ErrorViewBagMSJ("Please contact support");
                return View("ChangePassword");
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if (email != "")
            {
                bool userActive = false;
                string EmailAdreess = "";
                int userID;
                int token;
                bool verifyToken = false;

                try
                {
                    var userInfo = InventoryDB.spGetUserInfoByEmail(email).FirstOrDefault();
                    if (userInfo != null)
                    {
                        userActive = userInfo.userActive;
                        userID = userInfo.userID;

                        if (userActive)
                        {
                            //SET NEW TOKEN
                            Random rand = new Random();
                            token = rand.Next(10000000);
                            //VERIFY IF TOKEN EXIST
                            for (int i = 0; !verifyToken; i++)
                            {
                                var countToken = InventoryDB.spUserGetDuplicateToken(token).FirstOrDefault().Value;
                                if (countToken == 0 || countToken == null)
                                {
                                    verifyToken = true;
                                    //SET NEW TOKEN IN DATABASE
                                    InventoryDB.spUserSetNewToken(token, userID);
                                    //USCCESS MESSAGE
                                    SuccesViewBagMSJ("An email has been sent with the necessary information to restore your password.");
                                }
                            }

                            //SEND EMAIL - PENDING                       
                        }
                        else
                        {
                            ErrorViewBagMSJ("Please contact support!");
                        }
                    }
                    else
                    {
                        ErrorViewBagMSJ("No info found!");
                    }
                }
                catch (Exception ex)
                {
                    ErrorViewBagMSJ("Please contact support!");
                }

            }
            else
            {
                ErrorViewBagMSJ("Please contact support!");
            }
            return View();
        }

        private string ErrorViewBagMSJ(string msj)
        {
            ViewBag.Message = msj;
            ViewBag.Type = "1"; //Error
            return ViewBag.Message;
        }
        private string SuccesViewBagMSJ(string msj)
        {
            ViewBag.Message = msj;
            ViewBag.Type = "2"; //Succes
            return ViewBag.Message;
        }

        private string InfoViewBagMSJ(string msj)
        {
            ViewBag.Message = msj;
            ViewBag.Type = "3"; //Warning            
            return ViewBag.Message;
        }

        private string ErrorInactivityViewBagMSJ(string msj)
        {
            ViewBag.Message = msj;
            ViewBag.Type = "4"; //Inactivity
            return ViewBag.Message;
        }

        private string InfoResultViewBagMSJ(string msj)
        {
            ViewBag.Message = msj;
            ViewBag.Type = "5"; //Warning 2.0 - without button confirm
            return ViewBag.Message;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using InventoryManagement.Models;
using System.IO;

namespace InventoryManagement.Controllers
{
    public class CompanyController : Controller
    {
        INVENTORYMNGDBEntities INVENTORYMNGDB = new INVENTORYMNGDBEntities();
        MimeTypeMap mimeType = new MimeTypeMap();
        GeneralModels GeneralModels = new GeneralModels();
        // GET: Company
        public ActionResult Index()
        {
            try
            {
                ViewBag.CompanyInfo_result = INVENTORYMNGDB.spGetCompanyInformation(1).ToList();                
            }
            catch { }

            return View();
        }

        public ActionResult BranchOffices()
        {            
            return View();
        }

        public PartialViewResult _InsertUpdateCompany(int CID)
        {
            //CID == CompanyID

            spGetCompanyInformation_Result CompanyInfo_Result = new spGetCompanyInformation_Result();
            var CompanyInfo = INVENTORYMNGDB.spGetCompanyInformation(CID).FirstOrDefault();

            if (CompanyInfo == null)
            { CompanyInfo_Result.companyID = 0; }
            else { CompanyInfo_Result = CompanyInfo; }

            ViewBag.Country_result = INVENTORYMNGDB.spGetCountry().ToList();

            return PartialView("_InsertUpdateCompany", CompanyInfo_Result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult _InsertUpdateCompanyInfo(spGetCompanyInformation_Result fileData)
        {
            var CreatedBy = "1";
            var companyID = 0;
            var result = "";

            if (fileData.companyID > 0)
            {
                if (Request.Files.Count > 0)
                {
                    byte[] returnimageBytes = null;
                    string filename = "";
                    string filetype = "";
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        filename = Path.GetFileName(Request.Files[i].FileName);
                        filetype = mimeType.GetMimeType(Path.GetExtension(Request.Files[i].FileName).ToLower().Replace(".", "").Trim());
                        HttpPostedFileBase file = files[i];
                        if (filename != null && filename != "")
                        {
                            returnimageBytes = GeneralModels.ConvertToBytes(file);
                        }
                        else
                        {
                            returnimageBytes = null;
                        }

                        companyID = (int)INVENTORYMNGDB.spCompanyUpdate(fileData.companyID, fileData.countryID, fileData.companyName, 
                            fileData.companyAddress, fileData.companyContact, fileData.companyEmail, fileData.companyPhone, fileData.companyFax,
                            returnimageBytes, int.Parse(CreatedBy)).FirstOrDefault().Value;
                        result = "Updated";
                    }
                }
                else
                {
                    companyID = (int)INVENTORYMNGDB.spCompanyUpdate(fileData.companyID, fileData.countryID, fileData.companyName,
                            fileData.companyAddress, fileData.companyContact, fileData.companyEmail, fileData.companyPhone, fileData.companyFax,
                            null, int.Parse(CreatedBy)).FirstOrDefault().Value;
                    result = "Updated";
                }

            }
            else
            {
                if (Request.Files.Count > 0)
                {
                    byte[] returnimageBytes = null;
                    string filename = "";
                    string filetype = "";
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        filename = Path.GetFileName(Request.Files[i].FileName);
                        filetype = mimeType.GetMimeType(Path.GetExtension(Request.Files[i].FileName).ToLower().Replace(".", "").Trim());
                        HttpPostedFileBase file = files[i];
                        if (filename != null && filename != "")
                        {
                            returnimageBytes = GeneralModels.ConvertToBytes(file);
                        }
                        else
                        {
                            returnimageBytes = null;
                        }

                        companyID = (int)INVENTORYMNGDB.spCompanyInsert(fileData.countryID, fileData.companyName,
                            fileData.companyAddress, fileData.companyContact, fileData.companyEmail, fileData.companyPhone, fileData.companyFax,
                            returnimageBytes, int.Parse(CreatedBy)).FirstOrDefault().Value;
                        result = "Inserted";
                    }
                }
            }
            return Json(new { companyID = companyID, Message = result }, JsonRequestBehavior.AllowGet);
        }

    }
}
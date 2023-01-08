using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FileUpload.Models;
using System.Net;
using System.ComponentModel;

namespace FileUpload.Controllers
{
    public class DownloadFileController : Controller
    {
        
        [CheckSessionOut]
        public ActionResult Index() 
        {            

            ViewDataDownloadFinal f = new ViewDataDownloadFinal();
            int userid = (int)Session["UserID"];
            f.CategoryID = (Session["catId"] == null) ? 0 : (int)Session["catId"];

            ViewBag.categories = new SelectList(CategoryDAL.GetCategoryByUser(userid, "Download"), "CategoryID", "CategoryName", f.CategoryID);
            ViewBag.grouptype = new SelectList(GroupTypeDAL.GetGroupTypeList(), "intMstGroupType", "GroupTypeName");
            ViewBag.dept = new SelectList(DepartmentDAL.GetDeptList(), "intMstDept", "DeptName");
            return View(f);
        }
        
        //public FileResult Download(string id, int catId, string usr)
        public FileResult Download(string id, int catId, string usr)
        {
            DownloadCounter dlCnt = new DownloadCounter();
            var cat = CategoryDAL.GetCategory(catId);
            string folderPath = Path.Combine(Utilities.StorageRoot, id);
            string _str = string.Empty;
            //try
            //{

                byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath);
                string fileName = id;

                dlCnt.CategoryID = catId;
                dlCnt.DownloadedBy = usr;
                dlCnt.FileName = id;

                _str = DownloadDAL.AddCount(dlCnt);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                //if (_str.Contains("added"))
                //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                //else
                //    throw new Exception("Error on download.");
            //}
            //catch (Exception ex)
            //{                
            //    throw new Exception(ex.Message.ToString());
            //}            
        }
                
        [HttpGet, CheckSessionOut]
        public ActionResult GetFiles(int catId, int grouptype, int dept)
        {
            List<ViewDataDownloadedFilesFinal> list = new List<ViewDataDownloadedFilesFinal>();
            var userid = (int)Session["UserID"];            
            var files = UploadDAL.GetUploadList(catId, grouptype, dept, Session["Username"].ToString());

            foreach (var file in files)
            {
                list.Add(new ViewDataDownloadedFilesFinal()
                {
                    uploaddate = file.dataDate,
                    name = file.Subject,
                    desc = file.FileDescription,
                    size = file.FileSize,
                    filename = file.FileName,
                    url = "<a href=/DownloadFile/Download?id=" + file.FileName.ToString() + "&catId=" + catId.ToString() + "&usr=" + Session["Username"].ToString() + ">Download</a>",
                    //prevUrl = "<a target='_blank' href=/DownloadFile/PreviewFile?id=" + file.FileName.ToString() + ">Preview</a>"
                    prevUrl="<a href='#' onclick='document.getElementById('docFrame').contentWindow.location.href;' target='_blank'>Open Iframe Contents in a New Tab</a>"
                });
            }           

            string serial = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list);

            return Json(new { obj = serial }, JsonRequestBehavior.AllowGet);

        }

        private List<ViewDataDownloadedFiles> GetAllDownloadedFiles(Category cat)
        {
            var list = new List<ViewDataDownloadedFiles>();
            //var data = UploadDAL.GetUploadList(cat.CategoryID);

            //foreach (var item in data)
            //{
            //    list.Add(new ViewDataDownloadedFiles() 
            //    { 
            //        name=item.FileName,
            //        size = 0,
            //        type = "",
            //        url = "<a href=/DownloadFile/Download?id=" + item.FileName.ToString() + "&catId=" + cat.CategoryID + "&usr=" + Session["Username"].ToString() + ">Download</a>"
            //    });
            //}
            
            return list;

        }

        private void FillUserSession(User user)
        {
            Session["UserID"] = user.UserID;
            Session["Username"] = user.Username;
            Session["UserPassword"] = user.UserPassword;
            Session["intMstUserGroup"] = user.intMstUserGroup;
            Session["isActive"] = user.isActive;
            Session["isAdmin"] = user.isAdmin;
        }

        public ActionResult DownloadPDF()
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "inline; inline; filename=SkygoERP-DisbursementManual-BranchLevel.pdf");
            return File("~/Files/SkygoERP-DisbursementManual-BranchLevel.pdf", "application/pdf");
        }

        //public void PreviewFile(string id)
        //{
            
        //    string folderPath = Path.Combine(Utilities.StorageRoot, id);            
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath);
        //    FileInfo file = new FileInfo(folderPath);
        //    string fileName = id;

        //    Response.ClearContent();
        //    Response.AddHeader("Content-Disposition", "filename=" + fileName);
        //    Response.AddHeader("Content-Length", file.Length.ToString());
        //    Response.ContentType = "application/ms-word";

        //    file.IsReadOnly = true;
        //    Response.TransmitFile(file.FullName);
        //    //Response.WriteFile(folderPath);
        //    Response.End();
        //}
             
    }
}

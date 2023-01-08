using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FileUpload.Models;
using System.Data;
using System.Net;
using System.ComponentModel;

namespace FileUpload.Controllers
{
    public class UploadFileController : Controller
    {

        static List<string> fileNames;
        static int categoryId;
        static List<HttpPostedFileBase> colFiles;

        [CheckSessionOut]
        public ActionResult Index()
        {
            int userid = (int)Session["UserID"];

            ViewBag.categories = new SelectList(CategoryDAL.GetCategoryByUser(userid, "Upload"), "CategoryID", "CategoryName");
            return View();
        }

        [CheckSessionOut]
        public ActionResult FileDescription()
       {

           var data = ListOfFiles();
           ViewBag.dtDate = DateTime.Now.ToShortDateString();
           return View(data);

       }

        [CheckSessionOut]
        public ActionResult List()
       {
           int userid = (int)Session["UserID"];

           ViewBag.categories = new SelectList(CategoryDAL.GetCategoryByUser(userid, "Edit"), "CategoryID", "CategoryName");
           ViewBag.grouptype = new SelectList(GroupTypeDAL.GetGroupTypeList(), "intMstGroupType", "GroupTypeName");
           ViewBag.dept = new SelectList(DepartmentDAL.GetDeptList(), "intMstDept", "DeptName");

           return View();
       }

        [CheckSessionOut]
        public ActionResult SetCategory()
        {
            UploadSetCategory uplCat = new UploadSetCategory()
            {
                uplFiles = ListOfFiles(),
                catList = CategoryDAL.GetCategoryByUser((int)Session["UserID"], "Upload"),
                FileDescription = "",
                dataDate = DateTime.Now,
                FileName = ""
            };

            ViewBag.grouptype = new SelectList(GroupTypeDAL.GetGroupTypeCombo(), "intMstGroupType", "GroupTypeName");
            ViewBag.dept = new SelectList(DepartmentDAL.GetDeptList(), "intMstDept", "DeptName");

            return View(uplCat);
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
       {
           var upl = UploadDAL.GetUpload(id);
           upl.dataDate = DateTime.Now;

           ViewBag.grouptype = new SelectList(GroupTypeDAL.GetGroupTypeCombo(), "intMstGroupType", "GroupTypeName", upl.intMstGroupType);
           ViewBag.dept = new SelectList(DepartmentDAL.GetDeptList(), "intMstDept", "DeptName", upl.intMstDept);

           return View(upl);
       }

        [HttpPost, CheckSessionOut]
        public ActionResult Edit(HttpPostedFileBase xfiles, Upload upl)
        {

            string msg = string.Empty;
            string folderPath = Utilities.StorageRoot;
            DataTable dt = new DataTable();

            try
            {

                if (xfiles != null)
                {                    
                    if (Session["Username"] != null)
                    {
                        //delete oldfile
                        var filePath = Path.Combine(Utilities.StorageRoot, upl.FileName);
                        if (UploadDAL.FileIsUseByAnotherCategory(upl.FileName, upl.CategoryID) == false)
                        {
                            System.IO.File.Delete(filePath);
                        }

                        var fName = Utilities.TrimSpaces(xfiles.FileName.Replace("'", ""));
                        var fullPath = Path.Combine(folderPath, Path.GetFileName(fName));
                        double len = (double)((double)(xfiles.ContentLength / 1000) / 1000);
                        string[] val = len.ToString().Split('.');
                        var splitVal1 = val[0];
                        var splitDec = "0";

                        if (val.Length > 1)
                            splitDec = val[1].ToString().Substring(0, 2);

                        double size = Convert.ToInt32(splitVal1) + (Convert.ToDouble(splitDec) / 100);
                        xfiles.SaveAs(fullPath);

                        upl.OverrideBy = Session["Username"].ToString();
                        upl.FileSize = size.ToString() + "MB";
                        upl.isOverride = true;
                        upl.FileName = fName;                        
                    }                    
                }


                dt.Columns.Add("intTrnUpload");
                dt.Columns.Add("CategoryID");
                dt.Columns.Add("FileName");

                if (upl.FileDescription != string.Empty)
                {
                    dt.Rows.Add(upl.intTrnUpload, upl.CategoryID);

                    msg = UploadDAL.Save(upl, dt, Session["Username"].ToString());
                    return RedirectToAction("List");
                }
                else
                {
                    msg = "Please enter Description";
                }
                
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();                
            }

            ViewBag.grouptype = new SelectList(GroupTypeDAL.GetGroupTypeCombo(), "intMstGroupType", "GroupTypeName", upl.intMstGroupType);
            ViewBag.dept = new SelectList(DepartmentDAL.GetDeptList(), "intMstDept", "DeptName", upl.intMstDept);
            ViewBag.Msg = msg;
            return View();
        }

        [HttpPost, CheckSessionOut]
        public ActionResult Index(HttpPostedFileBase[] files, Category cat)
        {

            var category = CategoryDAL.GetCategory(cat.CategoryID);
            string folderPath;
            string msg = string.Empty;
            int userid = (int)Session["UserID"];            

            fileNames = new List<string>();
            colFiles = new List<HttpPostedFileBase>();
            categoryId = cat.CategoryID;

            try
            {                

                if (files[0] != null) 
                {                                        
                        //folderPath = Path.Combine(Utilities.StorageRoot, category.CategoryID.ToString());
                        folderPath = Utilities.StorageRoot;

                        //check if folder exists, if not create folder
                        if (System.IO.Directory.Exists(folderPath) == false)
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        //don't save if the session expired
                        if (Session["Username"] != null)
                        {
                            foreach (HttpPostedFileBase file in files)
                            {
                                var fileName = Utilities.TrimSpaces(file.FileName.Replace("'", ""));                                
                                var fullPath = Path.Combine(folderPath, Path.GetFileName(fileName));
                                double len = (double)((double)(file.ContentLength / 1000) / 1000);
                                string[] val = len.ToString().Split('.');
                                var splitVal1 = val[0];
                                //var splitDec = "0";

                                //if (val.Length > 1)
                                    //splitDec = val[1].ToString().Substring(0, 2);

                                //double size = Convert.ToInt32(splitVal1) + (Convert.ToDouble(splitDec) / 100);
                                var size = "";
                                file.SaveAs(fullPath);
                                colFiles.Add(file);
                                fileNames.Add(fileName + "~" + size);
                            }

                            
                            return RedirectToAction("SetCategory");                            
                        }                    
                }
                else
                {
                    msg = "No files to Upload.";
                }
                
                
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }


            ViewBag.categories = new SelectList(CategoryDAL.GetCategoryByUser(userid, "Upload"), "CategoryID", "CategoryName");
            ViewBag.Msg = msg;
            return View();            
        }

        [HttpPost, CheckSessionOut]
        public ActionResult SetCategory(UploadSetCategory uplCat)
        {
            DataTable dt = new DataTable();
            string msg = string.Empty;
            bool success = false;
            var data = new Upload();
            string folderPath;

            try
            {
                folderPath = Utilities.StorageRoot;

                dt.Columns.Add("intTrnUpload");
                dt.Columns.Add("CategoryID");
                dt.Columns.Add("FileName");

                foreach (var item in uplCat.catList)
                {
                    dt.Rows.Add(0, item.CategoryID, uplCat.FileName);
                }

                data.intTrnUpload = 0;
                data.FileName = uplCat.FileName;
                data.FileDescription = uplCat.FileDescription;
                data.Subject = uplCat.Subject;
                data.FileSize = uplCat.FileSize;
                data.intMstDept = uplCat.intMstDept;
                data.intMstGroupType = uplCat.intMstGroupType;
                data.dataDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                msg = UploadDAL.Save(data, dt, Session["Username"].ToString());
                if (msg.Contains("saved")) 
                { 
                    success = true;                    

                    foreach (string item in fileNames)
                    {
                        string fName = uplCat.FileName;
                        string[] splitItems = item.Split('~');
                        string itemName = splitItems[0];                        

                        if (fName == itemName)
                        {
                            //loop thru posted file base and save the file to server
                            foreach (HttpPostedFileBase file in colFiles)
                            {
                                var fileName = Utilities.TrimSpaces(file.FileName.Replace("'", ""));                                

                                if (fName == fileName)
                                {
                                    var fullPath = Path.Combine(folderPath, Path.GetFileName(fileName));

                                    file.SaveAs(fullPath);
                                    break;
                                }                                
                            }


                            fileNames.Remove(item);
                            break;
                        }
                    }
                    
                }

            }
            catch (Exception ex)
            {
                success = false;
                msg = ex.Message.ToString();
            }

            return Json(new { success = success.ToString(), msg = msg });

        }

        [CheckSessionOut, HttpGet]
        public ActionResult GetUploadList(int catId, int grouptype, int dept)
        {
            var data = UploadDAL.GetUploadList(catId, grouptype, dept, Session["Username"].ToString());

            string serial = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data);
            return Json(new { obj = serial }, JsonRequestBehavior.AllowGet);
        }

        [CheckSessionOut]
        public ActionResult Search()
        {
            var data = UploadDAL.GetUploadList(0, 0, 0, Session["Username"].ToString());

            return View(data);
        }

        [HttpPost]
        public ActionResult Delete(string id, int catId)
        {
            bool result = false;
            string msg = string.Empty;
            //var filePath = Path.Combine(Utilities.StorageRoot, catId.ToString(), id);
            var filePath = Path.Combine(Utilities.StorageRoot, id);

            try
            {
                //if (System.IO.File.Exists(filePath))
                //{
                if (UploadDAL.FileIsUseByAnotherCategory(id, catId) == false) {
                    System.IO.File.Delete(filePath);
                }

                msg = UploadDAL.Delete(id, catId);
                if (msg.Contains("deleted")) { result = true; }
                //}
            }
            catch (Exception ex)
            {
                result = false;
                msg = ex.Message.ToString();
            }

            return Json(new { result = result.ToString(), msg = msg });
        }

        private static List<Upload> ListOfFiles()
        {
            List<Upload> list = new List<Upload>();

            try
            {

                if (fileNames.Count > 0)
                {
                    foreach (string item in fileNames)
                    {
                        string[] splitStr = item.Split('~');

                        var _item = new Upload
                        {
                            intTrnUpload = 0,
                            CategoryID = categoryId,
                            FileName = splitStr[0],
                            FileDescription = "",
                            FileSize = splitStr[1]
                        };

                        list.Add(_item);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return list;

        }


    }
}

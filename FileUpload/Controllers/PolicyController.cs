using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUpload.Models;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Text.RegularExpressions;

namespace FileUpload.Controllers
{
    public class PolicyController : Controller
    {
        string strPath = System.Web.HttpContext.Current.Server.MapPath("~/Reports/");
        string folderPath = Utilities.PolicyStorageRoot;

        ReportsDAL rptDAL = new ReportsDAL();

        [CheckSessionOut]
        public ActionResult Upload()
        {
            var policy = new ViewPolicy() 
            { 
                poli = new Policy()
            };

            policy.poli.DateEffective = DateTime.Now;
            policy.poli.DocNo = PolicyDAL.GetLastMemoNo();
            policy.poli.isSecondSave = false;

            ViewBag.policytype = new SelectList(PolicyDAL.GetPolicyType(), "ValueMember", "DisplayMember");
            return View(policy);

        }

        [CheckSessionOut, HttpPost]
        public ActionResult Upload(ViewPolicy policy)
        {            
            string msg = string.Empty;

            ViewBag.policytype = new SelectList(PolicyDAL.GetPolicyType(), "ValueMember", "DisplayMember");

            try
            {
                if (policy.file.ContentType.ToString().Contains("pdf"))
                {
                    if (policy.file != null)
                    {

                        if (System.IO.Directory.Exists(folderPath) == false)
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        if (Session["Username"] != null)
                        {
                            var fileName = Utilities.TrimSpaces(policy.file.FileName.Replace("'", ""));
                            var fullPath = Path.Combine(folderPath, Path.GetFileName(fileName));

                            //if (Regex.IsMatch(fileName, @"^[a-zA-Z0-9\s.\?\,\'\;\:\#\@\$\%\^\&\*\!\-]+$"))
                            //{
                            //    msg = "Special Character on the filename is  not allowed. Please rename your file!";
                            //}
                            //else
                            //{


                                policy.poli.FileName = fileName;
                                msg = PolicyDAL.Save(policy.poli);
                                if (msg.Contains("saved"))
                                {
                                    policy.file.SaveAs(fullPath);
                                    policy.poli.isSecondSave = true;
                                    Session["policy"] = policy;
                                    return RedirectToAction("SendTo");
                                }
                                else
                                {
                                    ViewBag.Msg = msg;
                                    return View(policy);
                                }
                            }
                        //}
                    }
                    else
                    {
                        msg = "No file to upload.";
                    }
                }
                else
                {
                    msg = "PDF file only.";
                }                

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }

          
            ViewBag.Msg = msg;
            return View(policy);     
        }

        [CheckSessionOut]
        public ActionResult SendTo()
        {
            var emptySource = new List<ComboBoxSource>();
            var policy = (ViewPolicy)Session["policy"];

            policy.poli.PolicyDet = new List<PolicyDetail>();

            ViewBag.branches = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Branches"), "ValueMember", "DisplayMember");
            ViewBag.depts = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.pos = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.emps = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.policytype = new SelectList(PolicyDAL.GetPolicyType(), "ValueMember", "DisplayMember", policy.poli.PolicyType);

            string user = Session["Username"].ToString();
            GroupTaggingHeaderDLL obj = new GroupTaggingHeaderDLL();
            //Group Name
            DataSet ds = obj.BindGroupDDL(user);
            ViewBag.gname = ds.Tables[0];
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.gname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["GroupName"].ToString(), Value = @dr["ID"].ToString() });
            }
            ViewBag.GroupName = items;
            //Other Group
            DataSet ds1 = obj.BindSpecialGroupDDL();
            ViewBag.SpecialGroup = ds1.Tables[0];
            List<SelectListItem> items1 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.SpecialGroup.Rows)
            {
                items1.Add(new SelectListItem { Text = @dr["GroupName"].ToString(), Value = @dr["ID"].ToString() });
            }
            ViewBag.SpecialGroup = items1;

            //Branch Group
            DataSet ds2 = obj.BindBranchGroupDDL("","");
            ViewBag.BranchGroup = ds2.Tables[0];
            List<SelectListItem> items2 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.BranchGroup.Rows)
            {
                items2.Add(new SelectListItem { Text = @dr["BranchName"].ToString(), Value = @dr["BranchCode"].ToString() });
            }
            ViewBag.BranchGroup = items2;

            //Company Group
            DataSet ds3 = obj.BindCompanyGroupDDL("");
            ViewBag.CompanyGroup = ds3.Tables[0];
            List<SelectListItem> items3 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.CompanyGroup.Rows)
            {
                items3.Add(new SelectListItem { Text = @dr["CompanyCode"].ToString(), Value = @dr["intMstCompany"].ToString() });
            }
            ViewBag.CompanyGroup = items3;


            //Department Group
            DataSet ds4 = obj.BindDepartmentGroupDDL("", "", "");
            ViewBag.DepartmentGroup = ds4.Tables[0];
            List<SelectListItem> items4 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.DepartmentGroup.Rows)
            {
                items4.Add(new SelectListItem { Text = @dr["DepartmentName"].ToString(), Value = @dr["intMstDepartment"].ToString() });
            }
            ViewBag.DepartmentGroup = items4;

            //Position Group
            DataSet ds5 = obj.BindPositionGroupDDL("", "", "");
            ViewBag.PositionGroup = ds5.Tables[0];
            List<SelectListItem> items5 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.PositionGroup.Rows)
            {
                items5.Add(new SelectListItem { Text = @dr["PositionName"].ToString(), Value = @dr["intMstPosition"].ToString() });
            }
            ViewBag.PositionGroup = items5;

            //Employee Group
            DataSet ds6 = obj.BindEmployeeGroupDDL("", "", "", "");
            ViewBag.EmployeeGroup = ds6.Tables[0];
            List<SelectListItem> items6 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.EmployeeGroup.Rows)
            {
                items6.Add(new SelectListItem { Text = @dr["EmployeeName"].ToString(), Value = @dr["intMstEmpPersonal"].ToString() });
            }
            ViewBag.EmployeeGroup = items6;

            return View(policy);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult SendTo(Policy policy)
        {
            string msg = string.Empty;
            bool success = false;

            try
            {
                msg = PolicyDAL.UploadSave(policy);
                if (msg.Contains("saved"))
                {
                    success = true;
                    Session["policy"] = null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }

            return Json(new { success = success.ToString(), msg = msg });

        }

        [CheckSessionOut]
        public ActionResult Download()
        {
            var data = PolicyDAL.GetPolicyList();

            return View(data.ToList());
        }

        [CheckSessionOut]
        public FileResult DownloadFile(string id)
        {
            DownloadCounter dlCnt = new DownloadCounter();
            folderPath = Path.Combine(folderPath, id);

            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath);

            dlCnt.CategoryID = 0;
            dlCnt.FileName = id;
            dlCnt.DownloadedBy = Session["Username"].ToString();
            string strResult = DownloadDAL.AddCount(dlCnt);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, id);
        }

        [CheckSessionOut]
        public ActionResult List()
        {
            var data = PolicyDAL.GetPolicyList();

            return View(data.ToList());
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            var emptySource = new List<ComboBoxSource>();
            var policy = new ViewPolicy()
            {                
                poli = PolicyDAL.GetPolicy(id)
            };

            ViewBag.branches = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Branches"), "ValueMember", "DisplayMember");
            ViewBag.depts = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.pos = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.emps = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.policytype = new SelectList(PolicyDAL.GetPolicyType(), "ValueMember", "DisplayMember", policy.poli.PolicyType);

            string user = Session["Username"].ToString();
            GroupTaggingHeaderDLL obj = new GroupTaggingHeaderDLL();
            //Group Name
            DataSet ds = obj.BindGroupDDL(user);
            ViewBag.gname = ds.Tables[0];
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.gname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["GroupName"].ToString(), Value = @dr["ID"].ToString() });
            }
            ViewBag.GroupName = items;
            //Special Group
            DataSet ds1 = obj.BindSpecialGroupDDL();
            ViewBag.SpecialGroup = ds1.Tables[0];
            List<SelectListItem> items1 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.SpecialGroup.Rows)
            {
                items1.Add(new SelectListItem { Text = @dr["GroupName"].ToString(), Value = @dr["ID"].ToString() });
            }
            ViewBag.SpecialGroup = items1;

            //Branch Group
            DataSet ds2 = obj.BindBranchGroupDDL("","");
            ViewBag.BranchGroup = ds2.Tables[0];
            List<SelectListItem> items2 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.BranchGroup.Rows)
            {
                items2.Add(new SelectListItem { Text = @dr["BranchName"].ToString(), Value = @dr["BranchCode"].ToString() });
            }
            ViewBag.BranchGroup = items2;

            //Company Group
            DataSet ds3 = obj.BindCompanyGroupDDL("");
            ViewBag.CompanyGroup = ds3.Tables[0];
            List<SelectListItem> items3 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.CompanyGroup.Rows)
            {
                items3.Add(new SelectListItem { Text = @dr["CompanyCode"].ToString(), Value = @dr["intMstCompany"].ToString() });
            }
            ViewBag.CompanyGroup = items3;


            //Department Group
            DataSet ds4 = obj.BindDepartmentGroupDDL("", "", "");
            ViewBag.DepartmentGroup = ds4.Tables[0];
            List<SelectListItem> items4 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.DepartmentGroup.Rows)
            {
                items4.Add(new SelectListItem { Text = @dr["DepartmentName"].ToString(), Value = @dr["intMstDepartment"].ToString() });
            }
            ViewBag.DepartmentGroup = items4;

            //Position Group
            DataSet ds5 = obj.BindPositionGroupDDL("", "", "");
            ViewBag.PositionGroup = ds5.Tables[0];
            List<SelectListItem> items5 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.PositionGroup.Rows)
            {
                items5.Add(new SelectListItem { Text = @dr["PositionName"].ToString(), Value = @dr["intMstPosition"].ToString() });
            }
            ViewBag.PositionGroup = items5;

            //Employee Group
            DataSet ds6 = obj.BindEmployeeGroupDDL("", "", "", "");
            ViewBag.EmployeeGroup = ds6.Tables[0];
            List<SelectListItem> items6 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.EmployeeGroup.Rows)
            {
                items6.Add(new SelectListItem { Text = @dr["EmployeeName"].ToString(), Value = @dr["intMstEmpPersonal"].ToString() });
            }
            ViewBag.EmployeeGroup = items6;

            return View(policy);
        }

        [CheckSessionOut]
        public ActionResult Delete(string fileName, int intId)
        {
            string msg = string.Empty;
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                System.IO.File.Delete(filePath);
                msg = Utilities.DeleteFile(intId, fileName, "Policy");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return RedirectToAction("List");
        }

        public FileStreamResult PreviewFile(string id)
        {
            Response.AppendHeader("Content-Disposition", "inline; filename=" + id + ";");

            string path = Path.Combine(folderPath, id);
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        [HttpGet, CheckSessionOut]
        public ActionResult GetUser(string fileName)
        {
            var data = PolicyDAL.GetUsers(fileName);

            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, CheckSessionOut]
        public ActionResult GetPolicyList(bool hasEmptyString = false)
        {
            var data = PolicyDAL.GetComboPolicyList(hasEmptyString);

            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }

        [CheckSessionOut]
        public ActionResult ViewReport()
        {
            var emptySource = new List<ComboBoxSource>();

            ViewBag.notifs = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.users = new SelectList(emptySource, "ValueMember", "DisplayMember");

            NameOfUploader obj = new NameOfUploader();

            DataSet ds = obj.BindUploaderDDL();
            ViewBag.gname = ds.Tables[0];
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.gname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["EmployeeName"].ToString(), Value = @dr["intMstEmpPersonal"].ToString() });
            }
            ViewBag.UploadName = items;

            return View();
        }

        #region Reports
        public void ViewAMDReport(string fileName, string Username)
        {
            ReportDocument rpt = new ReportDocument();
            DataTable dt = new DataTable();
            string moduleName = "Memo Circular";

            try
            {
                strPath += "rptAMD.rpt";

                if (Username == "null")
                    Username = "";

                dt = rptDAL.GetAMDReport(fileName, Username, moduleName);

                rpt.Load(strPath);

                rpt.SetDataSource(dt);
                rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "AMDReport_" + moduleName);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            rpt.Close();
            rpt.Dispose();
        }

        public void ViewAMSReport(string fileName, DateTime AsofDate)
        {
            ReportDocument rpt = new ReportDocument();
            DataTable dt = new DataTable();
            string moduleName = "Memo Circular";

            try
            {
                strPath += "rptAMS.rpt";


                dt = rptDAL.GetAMSReport(fileName, AsofDate, moduleName);

                rpt.Load(strPath);

                rpt.SetDataSource(dt);
                rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "AMSReport_" + moduleName);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            rpt.Close();
            rpt.Dispose();
        }

        public void ViewDownloadTrail(string fileName, string userName)
        {
            ReportDocument rpt = new ReportDocument();
            DataTable dt = new DataTable();
            string moduleName = "Memo Circular";

            try
            {
                strPath += "rptDownloadTrail.rpt";

                if (userName == "null")
                    userName = "";

                dt = rptDAL.GetDownloadTrail(fileName, userName, moduleName);

                rpt.Load(strPath);

                rpt.SetDataSource(dt);
                rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "DownloadTrail");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            rpt.Close();
            rpt.Dispose();
        }

        public void ViewUploadTaggings(int ID)
        {
            ReportDocument rpt = new ReportDocument();
            DataTable dt = new DataTable();
            string moduleName = "Policy";

            try
            {
                strPath += "UploadTaggings.rpt";

                //if (userName == "null")
                //    userName = "";

                dt = rptDAL.GetUploadTaggings(ID, moduleName);

                rpt.Load(strPath);

                rpt.SetDataSource(dt);
                rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "UploadTaggings");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            rpt.Close();
            rpt.Dispose();

        }

        public void ViewUploadFilesSummaryReport(string userID, DateTime DateFrom, DateTime DateTo)
        {
            ReportDocument rpt = new ReportDocument();
            DataTable dt = new DataTable();
            string Type = "Memo Circular";

            try
            {
                strPath += "ViewUploadFilesSummary.rpt";

                dt = rptDAL.GetUploadFilesSummaryReport(userID, Type, DateFrom, DateTo);

                rpt.Load(strPath);

                rpt.SetDataSource(dt);
                rpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ViewUploadFilesSummary");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            rpt.Close();
            rpt.Dispose();

        }

        public JsonResult LoadDepartment(string CompanyID, string BranchID, string DepartmentID)
        {
            GroupTaggingHeaderDLL obj = new GroupTaggingHeaderDLL();

            DataSet ds1 = obj.BindDepartmentGroupDDL(CompanyID, BranchID, DepartmentID);
            ViewBag.SpecialGroup = ds1.Tables[0];
            List<SelectListItem> items1 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.SpecialGroup.Rows)
            {
                items1.Add(new SelectListItem { Text = @dr["DepartmentName"].ToString(), Value = @dr["intMstDepartment"].ToString() });
            }
            //ViewBag.SpecialGroup = items1;

            return Json(items1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadPosition(string CompanyID, string BranchID, string DepartmentID)
        {
            GroupTaggingHeaderDLL obj = new GroupTaggingHeaderDLL();

            DataSet ds1 = obj.BindPositionGroupDDL(CompanyID, BranchID, DepartmentID);
            ViewBag.SpecialGroup = ds1.Tables[0];
            List<SelectListItem> items1 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.SpecialGroup.Rows)
            {
                items1.Add(new SelectListItem { Text = @dr["PositionName"].ToString(), Value = @dr["intMstPosition"].ToString() });
            }
            //ViewBag.SpecialGroup = items1;

            return Json(items1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadEmployee(string CompanyID, string BranchID, string DepartmentID, string PositionID)
        {
            GroupTaggingHeaderDLL obj = new GroupTaggingHeaderDLL();

            DataSet ds1 = obj.BindEmployeeGroupDDL(CompanyID, BranchID, DepartmentID, PositionID);
            ViewBag.SpecialGroup = ds1.Tables[0];
            List<SelectListItem> items1 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.SpecialGroup.Rows)
            {
                items1.Add(new SelectListItem { Text = @dr["EmployeeName"].ToString(), Value = @dr["intMstEmpPersonal"].ToString() });
            }
            //ViewBag.SpecialGroup = items1;

            return Json(items1, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

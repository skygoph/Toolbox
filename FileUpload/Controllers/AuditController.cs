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
    public class AuditController : Controller
    {
        string strPath = System.Web.HttpContext.Current.Server.MapPath("~/Reports/");
        string folderPath = Utilities.EmpNoticeRoot;

        ReportsDAL rptDAL = new ReportsDAL();

        [CheckSessionOut]
        public ActionResult Upload()
        {
            var ntc = new ViewEmpNotice()
            {
                notice = new EmployeeNotice()
            };

            ntc.notice.EmpNoticeDate = DateTime.Now;
            ViewBag.noticetype = new SelectList(EmployeeNoticeDAL.GetAuditType(), "ValueMember", "DisplayMember");
            ViewBag.deviationtype = new SelectList(EmployeeNoticeDAL.GetDeviationType(), "ValueMember", "DisplayMember");

            return View(ntc);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult Upload(ViewEmpNotice not)
        {

            //string folderPath = string.Empty;
            string msg = string.Empty;

            ViewBag.noticetype = new SelectList(EmployeeNoticeDAL.GetAuditType(), "ValueMember", "DisplayMember");
            ViewBag.deviationtype = new SelectList(EmployeeNoticeDAL.GetDeviationType(), "ValueMember", "DisplayMember");

            try
            {
                if (not.file.ContentType.ToString().Contains("pdf"))
                {
                    if (not.file != null)
                    {
                        //folderPath = Utilities.EmpNoticeRoot;

                        if (System.IO.Directory.Exists(folderPath) == false)
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        if (Session["Username"] != null)
                        {
                            var fileName = Utilities.TrimSpaces(not.file.FileName.Replace("'", ""));
                            var fullPath = Path.Combine(folderPath, Path.GetFileName(fileName));

                            //if (Regex.IsMatch(fileName, @"^[a-zA-Z0-9\s.\?\,\'\;\:\#\@\$\%\^\&\*\!\-]+$"))
                            //{
                            //    msg = "Special Character on the filename is  not allowed. Please rename your file!";
                            //}
                            //else
                            //{

                                not.notice.FileName = fileName;
                                msg = EmployeeNoticeDAL.Save(not.notice);
                                if (msg.Contains("saved"))
                                {
                                    not.file.SaveAs(fullPath);
                                    Session["notice"] = not;
                                    return RedirectToAction("SendTo");
                                }
                                else
                                {
                                    ViewBag.Msg = msg;
                                    return View(not);
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
            return View(not);
        }

        [CheckSessionOut]
        public ActionResult SendTo()
        {
            var emptySource = new List<ComboBoxSource>();
            var not = (ViewEmpNotice)Session["notice"];

            not.notice.EmpNoticeDet = new List<EmployeeNoticeDetail>();

            ViewBag.branches = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Branches"), "ValueMember", "DisplayMember");
            ViewBag.depts = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.pos = new SelectList(emptySource, "ValueMember", "DisplayMember");
            //ViewBag.emps = new SelectList(emptySource, "ValueMember", "DisplayMember");

            ViewBag.emps = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Employees"), "ValueMember", "DisplayMember");

            //ViewBag.depts = new SelectList(MemoDAL.GetmemoData("", -1, 0, "Dept"), "ValueMember", "DisplayMember");
            //ViewBag.pos = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Positions"), "ValueMember", "DisplayMember");
            //ViewBag.emps = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Employees"), "ValueMember", "DisplayMember");
            ViewBag.noticetype = new SelectList(EmployeeNoticeDAL.GetAuditType(), "ValueMember", "DisplayMember", not.notice.intEmpNoticeType);
            ViewBag.deviationtype = new SelectList(EmployeeNoticeDAL.GetDeviationType(), "ValueMember", "DisplayMember");


            string user = Session["Username"].ToString();
            GroupTaggingHeaderDLL obj = new GroupTaggingHeaderDLL();

            DataSet ds = obj.BindGroupDDL(user);
            ViewBag.gname = ds.Tables[0];
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.gname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["GroupName"].ToString(), Value = @dr["ID"].ToString() });
            }
            ViewBag.GroupName = items;

            DataSet ds1 = obj.BindSpecialGroupDDL();
            ViewBag.SpecialGroup = ds1.Tables[0];
            List<SelectListItem> items1 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.SpecialGroup.Rows)
            {
                items1.Add(new SelectListItem { Text = @dr["GroupName"].ToString(), Value = @dr["ID"].ToString() });
            }
            ViewBag.SpecialGroup = items1;



            return View(not);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult SendTo(EmployeeNotice not)
        {
            string msg = string.Empty;
            bool success = false;

            try
            {
                msg = EmployeeNoticeDAL.SaveUpload(not);
                if (msg.Contains("saved"))
                {
                    success = true;
                    Session["notice"] = null;
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
            var data = EmployeeNoticeDAL.GetEmpNoticeList(1);

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
            var data = EmployeeNoticeDAL.GetEmpNoticeList(1);

            return View(data.ToList());
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            var emptySource = new List<ComboBoxSource>();
            var not = new ViewEmpNotice()
            {
                notice = EmployeeNoticeDAL.GetEmpNotice(id)
            };

            ViewBag.branches = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Branches"), "ValueMember", "DisplayMember");
            ViewBag.depts = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.pos = new SelectList(emptySource, "ValueMember", "DisplayMember");

            ViewBag.emps = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Employees"), "ValueMember", "DisplayMember");

            ViewBag.noticetype = new SelectList(EmployeeNoticeDAL.GetAuditType(), "ValueMember", "DisplayMember", not.notice.intEmpNoticeType);
            ViewBag.deviationtype = new SelectList(EmployeeNoticeDAL.GetDeviationType(), "ValueMember", "DisplayMember");

            

            string user = Session["Username"].ToString();
            GroupTaggingHeaderDLL obj = new GroupTaggingHeaderDLL();

            DataSet ds = obj.BindGroupDDL(user);
            ViewBag.gname = ds.Tables[0];
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.gname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["GroupName"].ToString(), Value = @dr["ID"].ToString() });
            }
            ViewBag.GroupName = items;

            DataSet ds1 = obj.BindSpecialGroupDDL();
            ViewBag.SpecialGroup = ds1.Tables[0];
            List<SelectListItem> items1 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.SpecialGroup.Rows)
            {
                items1.Add(new SelectListItem { Text = @dr["GroupName"].ToString(), Value = @dr["ID"].ToString() });
            }
            ViewBag.SpecialGroup = items1;

            return View(not);
        }

        [CheckSessionOut]
        public ActionResult Delete(string fileName, int intId)
        {
            string msg = string.Empty;
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                System.IO.File.Delete(filePath);
                msg = Utilities.DeleteFile(intId, fileName, "Employee Notice");
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
            var data = EmployeeNoticeDAL.GetUsers(fileName);

            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, CheckSessionOut]
        public ActionResult GetEmpNoticeList(bool hasEmptyString = false)
        {
            var data = EmployeeNoticeDAL.GetComboEmpNoticeList(hasEmptyString);

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
            string moduleName = "Employee Notice";

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
            string moduleName = "Employee Notice";

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
            string moduleName = "Employee Notice";

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
            string moduleName = "Notice";

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
            string Type = "Notice";

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

        #endregion

    }
}

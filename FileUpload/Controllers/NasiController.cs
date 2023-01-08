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
    public class NasiController : Controller
    {
        //
        // GET: /Nasi/

        string strPath = System.Web.HttpContext.Current.Server.MapPath("~/Reports/");
        string folderPath = Utilities.NotiStorageRoot;

        ReportsDAL rptDAL = new ReportsDAL();

        public ActionResult Upload()
        {
            var noti = new ViewNasi()
            {
                noti = new NasiAnnouncement()
            };
            noti.noti.DocNo = NotificationDAL.GetLastNotificationNo();
            noti.noti.isSecondSave = false;

            return View(noti);
        }


        [HttpPost, CheckSessionOut]
        public ActionResult Upload(ViewNasi notif)
        {
            string msg = string.Empty;

            try
            {

                if (notif.file.ContentType.ToString().Contains("pdf"))
                {

                    if (notif.file != null)
                    {
                        if (System.IO.Directory.Exists(folderPath) == false)
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        if (Session["Username"] != null)
                        {
                            var fileName = Utilities.TrimSpaces(notif.file.FileName.Replace("'", ""));
                            var fullPath = Path.Combine(folderPath, Path.GetFileName(fileName));


                            notif.noti.FileName = fileName;
                            msg = NasiNotificationDAL.Save(notif.noti);
                            if (msg.Contains("saved"))
                            {
                                notif.file.SaveAs(fullPath);
                                notif.noti.isSecondSave = true;
                                Session["noti"] = notif;
                                return RedirectToAction("SendTo");
                            }
                            else
                            {
                                ViewBag.Msg = msg;
                                return View(notif);
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
            return View(notif);

        }

    }
}

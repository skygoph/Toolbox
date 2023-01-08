using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUpload.Models;

namespace FileUpload.Controllers
{
    public class PrefixController : Controller
    {
        
        [CheckSessionOut]
        public ActionResult Index()
        {
            var list = DeptPrefixDAL.GetList();

            return View(list);
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            DeptPrefix dp = DeptPrefixDAL.Get(id);
            dp.intMstDepartment = id;

            ViewBag.Title = "Department Prefix";
            ViewBag.depts = new SelectList(DeptPrefixDAL.GetDepartment(), "ValueMember", "DisplayMember", id);
            return View(dp);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult Edit(DeptPrefix dp)
        {
            string msg = string.Empty;

            try
            {
                if ((dp.PrefixName == string.Empty) || (dp.PrefixName == null))
                {
                    msg = "Please don't leave the Prefix Name empty.";
                }
                else 
                {
                    msg = DeptPrefixDAL.Save(dp);
                }

                return Redirect("Index");
            }
            catch (Exception ex)
            {

                msg = ex.Message.ToString();
            }

            ViewBag.depts = new SelectList(DeptPrefixDAL.GetDepartment(), "ValueMember", "DisplayMember", dp.intMstDepartment);
            ViewBag.Title = "Department Prefix";
            ViewBag.Msg = msg;
            return View(dp);
        }

    }
}

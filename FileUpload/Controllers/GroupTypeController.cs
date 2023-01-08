using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUpload.Models;

namespace FileUpload.Controllers
{
    public class GroupTypeController : Controller
    {
       
        [CheckSessionOut]
        public ActionResult Index()
        {
            var gt = GroupTypeDAL.GetGroupTypeList();
            
            return View(gt);
        }

        [CheckSessionOut]
        public ActionResult Create()
        {
            GroupType gt = new GroupType();
            gt.intMstGroupType = 0;

            ViewBag.Title = "Add Group Type";
            return View(gt);
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            GroupType gt = GroupTypeDAL.GetGroupType(id);

            ViewBag.Title = "Update Group Type";
            return View("Create", gt);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult Create(GroupType gt)
        {
            string msg = string.Empty;

            try
            {
                if (gt.GroupTypeName == string.Empty)
                {
                    msg = "Please don't leave Group Type Name empty.";
                }
                else
                {
                    msg = GroupTypeDAL.Save(gt);
                }

                return Redirect("Index");

            }
            catch (Exception ex)
            {

                msg = ex.Message.ToString();
            }

            ViewBag.Msg = msg;
            return View(gt);
        }

        [CheckSessionOut]
        public ActionResult Delete(int id)
        {
            bool result = GroupTypeDAL.DeleteGroupType(id);

            if (result == true)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}

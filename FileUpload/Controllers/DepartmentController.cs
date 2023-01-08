using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUpload.Models;

namespace FileUpload.Controllers
{
    public class DepartmentController : Controller
    {
        [CheckSessionOut]
        public ActionResult Index()
        {
            var dept = DepartmentDAL.GetDeptList();

            return View(dept);
        }

        [CheckSessionOut]
        public ActionResult Create()
        {
            Department dept = new Department();
            dept.intMstDept = 0;

            ViewBag.Title = "Add Department";
            return View(dept);
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            var dept = DepartmentDAL.GetDept(id);

            ViewBag.Title = "Update Department";
            return View("Create", dept);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult Create(Department dept)
        {
            string msg = string.Empty;

            try
            {
                if (dept.DeptName == string.Empty)
                {
                    msg = "Please don't leave Department empty.";
                }
                else
                {
                    msg = DepartmentDAL.Save(dept);
                }

                return Redirect("Index");

            }
            catch (Exception ex)
            {

                msg = ex.Message.ToString();
            }

            ViewBag.Msg = msg;
            return View(dept);
        }

        [CheckSessionOut]
        public ActionResult Delete(int id)
        {
            bool result = DepartmentDAL.DeleteDept(id);

            if (result == true)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}

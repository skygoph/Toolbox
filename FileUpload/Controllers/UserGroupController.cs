using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUpload.Models;

namespace FileUpload.Controllers
{
    public class UserGroupController : Controller
    {
        [CheckSessionOut]
        public ActionResult Index()
        {
            var list = UserDAL.GetUserGroupList();

            ViewBag.Title = "User Group";
            return View(list.ToList());
        }

        [CheckSessionOut]
        public ActionResult Create()
        {
            var data = new UserGroup();
            data.UserGroupDetail = UserDAL.GetUserGroupDetail(0);
            data.ModuleDetail = UserDAL.GetModuleDetail(0);

            var cat = CategoryDAL.GetCategoryList();

            if (data.UserGroupDetail.Count == 0)
            {
                foreach (var item in cat)
                {
                    var x = new UserGroupDetailViewData 
                    { 
                        intMstUserGroupDetail = 0,
                        intMstUserGroup = data.intMstUserGroup,
                        CategoryID = item.CategoryID,
                        CategoryName = item.CategoryName,
                        canUpload = false,
                        canDownload = false
                    };

                    data.UserGroupDetail.Add(x);
                }
            }

            ViewBag.Title = "Add Group";
            return View(data);
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            var data = UserDAL.GetUserGroup(id);

            ViewBag.Title = "Update Group";
            return View("Create", data);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult SaveRecord(UserGroup group)
        {
            string msg = string.Empty;
            bool success = false;

            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                System.Data.DataTable dtM = new System.Data.DataTable();

                dt.Columns.Add("intMstUserGroupDetail");
                dt.Columns.Add("intMstUserGroup");
                dt.Columns.Add("CategoryID");
                dt.Columns.Add("canUpload");
                dt.Columns.Add("canDownload");
                dt.Columns.Add("canEdit");

                dtM.Columns.Add("intMstModuleDetail");
                dtM.Columns.Add("intMstModule");
                dtM.Columns.Add("intMstUserGroup");
                dtM.Columns.Add("canUpload");
                dtM.Columns.Add("canDownload");
                dtM.Columns.Add("canEdit");

                if (group.UserGroupDetail != null) {
                    foreach (var item in group.UserGroupDetail)
                    {
                        dt.Rows.Add(item.intMstUserGroupDetail, item.intMstUserGroup, item.CategoryID, item.canUpload, item.canDownload, item.canEdit);
                    }
                }

                if (group.ModuleDetail != null) {
                    foreach (var item in group.ModuleDetail)
                    {
                        dtM.Rows.Add(item.intMstModuleDetail, item.intMstModule, item.intMstUserGroup, item.canUpload, item.canDownload, item.canEdit);
                    }
                }                

                msg = UserDAL.SaveUserGroup(group, dt, dtM);
                if (msg.Contains("saved")) { success = true; }

            }
            catch (Exception ex)
            {
                success = false;
                msg = ex.Message.ToString();
            }

            return Json(new { success = success.ToString(), msg = msg });

        }

        [CheckSessionOut]
        public ActionResult Delete(int id)
        {
            bool result = UserDAL.DeleteUserGroup(id);

            if (result == true)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

    }
}

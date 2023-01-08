using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUpload.Models;

namespace FileUpload.Controllers
{
    public class CategoryController : Controller
    {        

        [CheckSessionOut]
        public ActionResult Index()
        {
            var cat = CategoryDAL.GetCategoryList();
            
            return View(cat.ToList());
        }

        [CheckSessionOut]
        public ActionResult Create()
        {
            Category cat = new Category();
            cat.CategoryID = 0;

            ViewBag.Title = "Add Category";
            return View(cat);
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            var category = CategoryDAL.GetCategory(id);

            ViewBag.Title = "Update Category";
            return View("Create", category);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult Create(Category cat)
        {
            string msg = string.Empty;

            try
            {
                if (cat.CategoryName == string.Empty)
                {
                    msg = "Please don't leave Category Description";
                }
                else
                {
                    msg = CategoryDAL.Save(cat);
                }

                return Redirect("Index");

            }
            catch (Exception ex)
            {

                msg = ex.Message.ToString();
            }

            ViewBag.Msg = msg;
            return View(cat);
            
        }

        [CheckSessionOut]
        public ActionResult Delete(int id)
        {

            if (CategoryDAL.isCategoryUsed(id) == false) {
                bool result = CategoryDAL.DeleteCategory(id);
            }

            return RedirectToAction("Index");
        }
    }
}

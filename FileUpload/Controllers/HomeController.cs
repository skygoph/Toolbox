using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginVerification;

namespace FileUpload.Controllers
{
    public class HomeController : Controller
    {        

        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();            
        }
        public ActionResult TestData()
        {
            return View();
        }
    }
}

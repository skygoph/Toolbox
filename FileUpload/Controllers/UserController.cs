using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUpload.Models;
using LoginVerification;

namespace FileUpload.Controllers
{
    public class UserController : Controller
    {

        LoginVerify _LoginVerify = new LoginVerify();

        [CheckSessionOut]
        public ActionResult Index()
        {
            var list = UserDAL.GetUserList();

            ViewBag.Title = "User Rights";
            return View(list.ToList());
        }

        [CheckSessionOut]
        public ActionResult Create()
        {
            var data = new User();
            data.UserID = 0;
            data.Username = "";
            data.UserPassword = "12345";
            data.isActive = false;
            data.isAdmin = false;

            ViewBag.usergroup = new SelectList(UserDAL.GetUserGroupList(), "intMstUserGroup", "UserGroupName");
            ViewBag.Title = "Add User";
            return View(data);
        }

        [CheckSessionOut]
        public ActionResult Edit(int id)
        {
            //var userId = Session["UserID"].ToString();
            var data = UserDAL.GetUserOneAccess(id);

            ViewBag.usergroup = new SelectList(UserDAL.GetUserGroupList(), "intMstUserGroup", "UserGroupName", data.intMstUserGroup);
            ViewBag.Title = "Update User";
            return View("Create", data);

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User myUser)
        {
            string strMsg = string.Empty;

            try
            {
                var user = UserDAL.AuthenticateUser(myUser.Username, myUser.UserPassword);

                if ((user.Username != "") && (user.Username != null))
                {
                    if (user.isActive == false)
                    {
                        strMsg = "User is not active.";
                    }
                    else
                    {
                        FillUserSession(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    strMsg = "Invalid username and password.";
                }

            }
            catch (Exception ex)
            {
                //error page here
                strMsg = ex.Message.ToString();
            }

            ViewBag.ErrorMsg = strMsg;
            //return View(myUser);

            string sysLinkONEAccess = _LoginVerify.GetSysLinkONEAccess();
            return Redirect(sysLinkONEAccess);
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["Username"] = null;
            Session["UserPassword"] = null;
            Session["intMstUserGroup"] = null;
            Session["isActive"] = null;
            Session["isAdmin"] = null;
            Session["hasGroupTaggings"] = null;
            Session["sysAdmin"] = null;
            Session["sysModules"] = null;

            Session.RemoveAll();
            string sysLinkONEAccess = _LoginVerify.GetSysLinkONEAccess();
            return Redirect(sysLinkONEAccess);
        }

        [HttpPost, CheckSessionOut]
        public ActionResult Create(UserOneAccess user)
        {
            string msg = string.Empty;

            try
            {
                //if (UserDAL.CheckUserExists(user.Username) == true)
                //{
                //    msg = "User already exists.";
                //}
                //else
                //{
                //    msg = UserDAL.SaveUser(user);
                //}

                msg = UserDAL.SaveUser(user);
                return Redirect("Index");
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }

            ViewBag.Msg = msg;
            return View(user);

        }

        [CheckSessionOut]
        public ActionResult Delete(int id)
        {
            bool result = UserDAL.DeleteUser(id);

            if (result == true)
            {
                return RedirectToAction("Index");
            }

            
            return View();
        }

        private void FillUserSession(User user)
        {            
            Session["UserID"] = user.UserID;
            Session["Username"] = user.Username;
            Session["UserPassword"] = user.UserPassword;
            Session["intMstUserGroup"] = user.intMstUserGroup;
            Session["isActive"] = user.isActive;
            Session["isAdmin"] = user.isAdmin;
            Session["hasGroupTaggings"] = user.hasGroupTaggings;

            Session["intMstDepartment"] = Utilities.GetDepartmentID(user.Username);
            Session["sysAdmin"] = Utilities.DBLookup("SELECT strAdminName FROM tblMstAdmin WHERE intMstAdmin = 1").ToString();
            Session["sysModules"] = UserDAL.GetModuleDetail(user.intMstUserGroup);
            Session["sysCategories"] = UserDAL.GetUserGroupDetail(user.intMstUserGroup);
        }

        [CheckSessionOut]
        public ActionResult ChangePassword()
        {
            ChangePassword cp = new ChangePassword();
            cp.Username = Session["Username"].ToString();

            ViewBag.Title = "Change Password";
            return View(cp);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword cp)
        {
            string _msg = string.Empty;
            bool result = false;

            if (cp.CurrentPassword != Session["UserPassword"].ToString())
            {
                _msg = "Password is not correct.";
            }
            else if (cp.ConfirmPassword != cp.NewPassword)
            {
                _msg = "New Password does not match.";
            }
            else
            {
                _msg = UserDAL.ChangePassowrd(cp);
                if (_msg.Contains("change")) 
                {
                    result = true;
                    Session["UserPassword"] = cp.NewPassword;
                }
                
            }            

            ViewBag.Msg = _msg;
            if (result == true)
            {
                return View(cp);
            }
            return View();
        }

        public ActionResult LoginUser(string stringA, string stringB, int right, int intMstSysMasterFile, int ComNo, bool isShortCut = false)
        {
            string strMsg = string.Empty;

            if (_LoginVerify.VerifyUsername(stringA, stringB))
            {
                try
                {
                    var user = UserDAL.AuthenticateUser(stringA);

                    if ((user.Username != "") && (user.Username != null))
                    {
                        user.intMstUserGroup = right;
                        Session["intMstSysMasterfile"] = intMstSysMasterFile;
                        FillUserSession(user);


                        if (isShortCut == true)
                        {
                            if (ComNo == 1)//1 stands for Department Order                            
                                return RedirectToAction("Download", "Memo");
                            
                            else if (ComNo == 2)//2 stands for Announcements
                                return RedirectToAction("Download", "Notification");

                            else if (ComNo == 3)//3 stands for Emp Notice
                                return RedirectToAction("Download", "EmployeeNotice");

                            else if (ComNo == 4)
                                return RedirectToAction("Download", "Policy");

                        }

                        //if you reach here the command is log in only
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string sysLinkONEAccess = _LoginVerify.GetSysLinkONEAccess();
                        return Redirect(sysLinkONEAccess);
                    }

                }
                catch (Exception)
                {
                    string sysLinkONEAccess = _LoginVerify.GetSysLinkONEAccess();
                    // REDIREC TO LINK
                    return Redirect(sysLinkONEAccess);
                }

            }
            else
            {               
                // GET ONE ACCESS LINK
                string sysLinkONEAccess = _LoginVerify.GetSysLinkONEAccess();

                // REDIREC TO LINK
                return Redirect(sysLinkONEAccess);
            }
            
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FileUpload.Models;

namespace FileUpload.Controllers
{
    public class MasterfileController : Controller
    {
        //
        // GET: /Masterfile/
        
        public ActionResult Index()
        {
            GroupTagging GroupTagHeader = new GroupTagging();

            string user = Session["Username"].ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString.ToString()))
            {
                DataSet dset6 = new DataSet();

                using (SqlCommand cmd = new SqlCommand("spGetGroupNameTagging", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user", user);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dset6);

                    List<GroupTaggingHeader> GroupTaggingHeaders = new List<GroupTaggingHeader>();

                    for (int i = 0; i < dset6.Tables[0].Rows.Count; i++)
                    {
                        GroupTaggingHeader GroupTaggingHeader = new GroupTaggingHeader();

                        GroupTaggingHeader.GroupID = Convert.ToInt32(dset6.Tables[0].Rows[i]["ID"].ToString());
                        GroupTaggingHeader.GroupName = dset6.Tables[0].Rows[i]["GroupName"].ToString();
                       
                        GroupTaggingHeaders.Add(GroupTaggingHeader);
                    }
                    GroupTagHeader.GroupTagHead = GroupTaggingHeaders;
                }
                con.Close();
            }


            return View(GroupTagHeader);
        }

        public ActionResult AddGroupTagging(GroupTaggingDAL obj)
        {



            var emptySource = new List<ComboBoxSource>();
            var vm = (GroupDetails)Session["vm"];
   
            ViewBag.branches = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Branches"), "ValueMember", "DisplayMember");
            ViewBag.pos = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.depts = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.emps = new SelectList(emptySource, "ValueMember", "DisplayMember");


            //OTHER DROP DOWN
            DataSet ds = obj.BindDDL();
            ViewBag.fname = ds.Tables[0];
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["strCompleteName"].ToString(), Value = @dr["EmployeeID"].ToString() });
            }
            ViewBag.firstname = items;

            ////BRANCH DROP DOWN
            //DataSet dsB = obj.BindDDLBranch();
            //ViewBag.branch = dsB.Tables[0];
            //List<SelectListItem> item1 = new List<SelectListItem>();
            //foreach (System.Data.DataRow dr in ViewBag.branch.Rows)
            //{
            //    item1.Add(new SelectListItem { Text = @dr["BranchName"].ToString(), Value = @dr["intMstBranch"].ToString() }); 
            //}
            //ViewBag.BranchName = item1;

            ////POSITION DROPDOWN
            //DataSet dsP = obj.BindDDLPositon();
            //ViewBag.Position = dsP.Tables[0];
            //List<SelectListItem> item2 = new List<SelectListItem>();
            //foreach (System.Data.DataRow dr in ViewBag.Position.Rows)
            //{
            //    item2.Add(new SelectListItem { Text = @dr["PositionName"].ToString(), Value = @dr["intMstPosition"].ToString() });
            //}
            //ViewBag.Position = item2;

            ////DEPARTMENT DROP DOWN
            //DataSet dsD = obj.BindDDLDepartment();
            //ViewBag.Department = dsD.Tables[0];
            //List<SelectListItem> item3 = new List<SelectListItem>();
            //foreach (System.Data.DataRow dr in ViewBag.Department.Rows)
            //{
            //    item3.Add(new SelectListItem { Text = @dr["DepartmentName"].ToString(), Value = @dr["intMstDepartment"].ToString() });
            //}
            //ViewBag.Department = item3;

            GroupTaggingHeaderDLL objs= new GroupTaggingHeaderDLL();

            //Employee Group
            DataSet ds6 = objs.BindEmployeeGroupDDL("", "", "", "");
            ViewBag.EmployeeGroup = ds6.Tables[0];
            List<SelectListItem> items6 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.EmployeeGroup.Rows)
            {
                items6.Add(new SelectListItem { Text = @dr["EmployeeName"].ToString(), Value = @dr["intMstEmpPersonal"].ToString() });
            }
            ViewBag.EmployeeGroup = items6;

            //Branch Group
            DataSet ds2 = objs.BindBranchGroupDDL("", "");
            ViewBag.BranchGroup = ds2.Tables[0];
            List<SelectListItem> items2 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.BranchGroup.Rows)
            {
                items2.Add(new SelectListItem { Text = @dr["BranchName"].ToString(), Value = @dr["BranchCode"].ToString() });
            }
            ViewBag.BranchGroup = items2;

            //Position Group
            DataSet ds5 = objs.BindPositionGroupDDL("", "", "");
            ViewBag.PositionGroup = ds5.Tables[0];
            List<SelectListItem> items5 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.PositionGroup.Rows)
            {
                items5.Add(new SelectListItem { Text = @dr["PositionName"].ToString(), Value = @dr["intMstPosition"].ToString() });
            }
            ViewBag.PositionGroup = items5;

            //Department Group
            DataSet ds4 = objs.BindDepartmentGroupDDL("", "", "");
            ViewBag.DepartmentGroup = ds4.Tables[0];
            List<SelectListItem> items4 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.DepartmentGroup.Rows)
            {
                items4.Add(new SelectListItem { Text = @dr["DepartmentName"].ToString(), Value = @dr["intMstDepartment"].ToString() });
            }
            ViewBag.DepartmentGroup = items4;

        

            return View(vm);
        }

        public ActionResult SaveGroupTag(GroupTaggingHeader GroupHeader)
        {

            SaveTagging savetag = new SaveTagging();

            string result = savetag.SavingTags(GroupHeader);

            if (result != "Insert")
            {
                return Json(new { success = false, responseText = "You have Failed to save your Group Tagging! Please check the missing field" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, responseText = "You have successfully Created a Group Tagging!" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult EditGroupTaggings(int ID)
        {

            GroupTagging Grouptags = new GroupTagging();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString.ToString()))
            {
                DataSet dset = new DataSet();

                using (SqlCommand cmd = new SqlCommand("spEditGroupingTags", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GroupID", ID);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dset);

                    List<GroupTaggingHeader> GroupTaggingHeaders = new List<GroupTaggingHeader>();

                    for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                    {
                        GroupTaggingHeader GroupTaggingHeader = new GroupTaggingHeader();

                        GroupTaggingHeader.GroupID = Convert.ToInt32(dset.Tables[0].Rows[i]["ID"].ToString());
                        GroupTaggingHeader.GroupName = dset.Tables[0].Rows[i]["GroupName"].ToString();
                        

                        GroupTaggingHeaders.Add(GroupTaggingHeader);
                    }
                    Grouptags.GroupTagHead = GroupTaggingHeaders;
                }
                con.Close();
            }


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString.ToString()))
            {
                DataSet dset = new DataSet();

                using (SqlCommand cmd = new SqlCommand("spGroupTagsDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GroupID", ID);

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dset);

                    List<GroupTaggingDetail> GroupTaggingDetails = new List<GroupTaggingDetail>();

                    for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                    {
                        GroupTaggingDetail GroupTaggingDetail = new GroupTaggingDetail();

                        GroupTaggingDetail.intTrnGroupDetail = Convert.ToInt32(dset.Tables[0].Rows[i]["intTrnGroupDetail"].ToString());
                        GroupTaggingDetail.BranchCode = dset.Tables[0].Rows[i]["BranchCode"].ToString();
                        GroupTaggingDetail.Branch = dset.Tables[0].Rows[i]["BranchName"].ToString();
                        GroupTaggingDetail.intMstDepartment = Convert.ToInt32(dset.Tables[0].Rows[i]["intMstDepartment"].ToString());
                        GroupTaggingDetail.Department = dset.Tables[0].Rows[i]["DepartmentName"].ToString();
                        GroupTaggingDetail.intMstPosition = Convert.ToInt32(dset.Tables[0].Rows[i]["intMstPosition"].ToString());
                        GroupTaggingDetail.Position = dset.Tables[0].Rows[i]["PositionName"].ToString();
                        GroupTaggingDetail.intMstEmpPersonal = dset.Tables[0].Rows[i]["intMstEmpPersonal"].ToString();
                        GroupTaggingDetail.Employee = dset.Tables[0].Rows[i]["Employee"].ToString();

                        GroupTaggingDetails.Add(GroupTaggingDetail);
                    }
                    Grouptags.GroupDetail = GroupTaggingDetails;
                }
                con.Close();
            }

            var emptySource = new List<ComboBoxSource>();

            ViewBag.branches = new SelectList(MemoDAL.GetmemoData("", 0, 0, "Branches"), "ValueMember", "DisplayMember");
            ViewBag.pos = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.depts = new SelectList(emptySource, "ValueMember", "DisplayMember");
            ViewBag.emps = new SelectList(emptySource, "ValueMember", "DisplayMember");


            GroupTaggingDAL obj = new GroupTaggingDAL();

            DataSet ds = obj.BindDDL();
            ViewBag.fname = ds.Tables[0];
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["strCompleteName"].ToString(), Value = @dr["EmployeeID"].ToString() });
            }
            ViewBag.firstname = items;

            //BRANCH DROP DOWN
            DataSet dsB = obj.BindDDLBranch();
            ViewBag.branch = dsB.Tables[0];
            List<SelectListItem> item1 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.branch.Rows)
            {
                item1.Add(new SelectListItem { Text = @dr["BranchName"].ToString(), Value = @dr["intMstBranch"].ToString() });
            }
            ViewBag.BranchName = item1;

            //POSITION DROPDOWN
            DataSet dsP = obj.BindDDLPositon();
            ViewBag.Position = dsP.Tables[0];
            List<SelectListItem> item2 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.Position.Rows)
            {
                item2.Add(new SelectListItem { Text = @dr["PositionName"].ToString(), Value = @dr["intMstPosition"].ToString() });
            }
            ViewBag.Position = item2;

            //DEPARTMENT DROP DOWN
            DataSet dsD = obj.BindDDLDepartment();
            ViewBag.Department = dsD.Tables[0];
            List<SelectListItem> item3 = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.Department.Rows)
            {
                item3.Add(new SelectListItem { Text = @dr["DepartmentName"].ToString(), Value = @dr["intMstDepartment"].ToString() });
            }
            ViewBag.Department = item3;

            return View(Grouptags);
        }

        public ActionResult SaveEditGroupTagging(GroupTaggingHeader GroupHeader)
        {
            SaveEditTagging saveEditTags = new SaveEditTagging();

            string result = saveEditTags.SaveEditTags(GroupHeader);

            if (result != "Insert")
            {
                return Json(new { success = false, responseText = "You have successfully Updated your Group Tagging!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, responseText = "You have successfully Created a Group Tagging!" }, JsonRequestBehavior.AllowGet);
            }        

        }

        public ActionResult DeleteGroupTaggings(int ID)
        {
            DeleteGroupTags DelTags = new DeleteGroupTags();

            string result = DelTags.DelGroupTags(ID);

            //if (result != "Delete")
            //{
            //    return Json(new { success = false, responseText = "Failed to Delete Group Tagging!" }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { success = true, responseText = "You have successfully Deleted Group Tagging!" }, JsonRequestBehavior.AllowGet);
            //} 


            return RedirectToAction("Index", "Masterfile");
                 
        }

        public ActionResult UploadReports()
        {
            return View();
        }


       

    }
}

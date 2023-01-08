using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FileUpload.Models
{
    public class GroupTagging
    {     
        public List<GroupTaggingHeader> GroupTagHead { get; set; }
        public IEnumerable<GroupTaggingDetail> GroupDetail { get; set; }
        public IEnumerable<GroupDetails> GroupDetails { get; set; }
    }

    public class GroupTaggingHeader
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string intMstPersonal { get; set; }
        public int status { get; set; }
        public virtual ICollection<GroupDetails> Groupdet { get; set; }
    }

    public class GroupTaggingDetail
    {
        //public int ID { get; set; }
        //public string EmployeeID { get; set; }
        //public string EmployeeName { get; set; }
        public int intTrnGroupDetail { get; set; }
        public string BranchCode { get; set; }
        public string Branch { get; set; }
        public int intMstDepartment { get; set; }
        public string Department { get; set; }
        public int intMstPosition { get; set; }
        public string Position { get; set; }
        public string intMstEmpPersonal { get; set; }
        public string Employee { get; set; }
    }

    public class GroupTaggingComboBox
    {
        public object GroupValue { get; set; }
        public string GroupDisplayValue { get; set; }
    }

    public class GroupDetails
    {
        public GroupTaggingHeader intTrnGroupDetail { get; set; }
        public int intGroupHeadID { get; set; }
        public string BranchCode { get; set; }
        public int intMstDepartment { get; set; }
        public int intMstPosition { get; set; }
        public string intMstEmpPersonal { get; set; }

        
    }

    public class GroupTaggingDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionHRIS"].ConnectionString);

        public DataSet BindDDL()
        {           
            SqlCommand cmd = new SqlCommand("spGroupTagEmployeeList", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet BindDDLBranch()
        {
            SqlCommand cmd = new SqlCommand("spGroupInsuranceBranchMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet BindDDLPositon()
        {
            SqlCommand cmd = new SqlCommand("Masterfile_GetPositions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet BindDDLDepartment()
        {
            SqlCommand cmd = new SqlCommand("Masterfile_GetDepartments", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
    }

    public class GroupTaggingHeaderDLL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);

        public DataSet BindGroupDDL(string user)
        {
            SqlCommand cmd = new SqlCommand("spGetGroupTaggingHeader", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@user", user);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet BindSpecialGroupDDL()
        {
            SqlCommand cmd = new SqlCommand("spGetSpecialGroup", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@user", user);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet BindBranchGroupDDL(string CompanyID, string BranchID)
        {
            SqlCommand cmd = new SqlCommand("spGetBranchDDL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public DataSet BindDepartmentGroupDDL(string CompanyID, string BranchID, string DepartmentID)
        {
            SqlCommand cmd = new SqlCommand("spGetDapartmentDDL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@DepartmentID", DepartmentID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet BindPositionGroupDDL(string CompanyID, string BranchID, string DepartmentID)
        {
            SqlCommand cmd = new SqlCommand("spGetPositionDDL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DepartmentID", DepartmentID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet BindEmployeeGroupDDL(string CompanyID, string BranchID, string DepartmentID, string PositionID)
        {
            SqlCommand cmd = new SqlCommand("spGetEmployeeDDL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@DepartmentID", DepartmentID);
            cmd.Parameters.AddWithValue("@PositionID", PositionID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        //company
        public DataSet BindCompanyGroupDDL(string CompanyID)
        {
            SqlCommand cmd = new SqlCommand("spGetCompanyDDL", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

    }

    public class NameOfUploader
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);

        public DataSet BindUploaderDDL()
        {
            SqlCommand cmd = new SqlCommand("spGetEmpUploader", con);
            cmd.CommandType = CommandType.StoredProcedure;
           // cmd.Parameters.AddWithValue("@user", user);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
    }

    public class SaveTagging
    {
       
       SqlConnection cons = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
        string result = "";

        public string SavingTags(GroupTaggingHeader GroupHeader)
            {
                var dbMgr = new dbManager();
                string strMsg = string.Empty;
                DataTable dt = new DataTable();

                dt.Columns.Add("intTrnGroupDetail");
                dt.Columns.Add("intGroupHeadID");
                dt.Columns.Add("BranchCode");
                dt.Columns.Add("intMstDepartment");
                dt.Columns.Add("intMstPosition");
                dt.Columns.Add("intMstEmpPersonal"); 


                try
                {

                    if (GroupHeader.Groupdet != null)
                    {
                        foreach (var item in GroupHeader.Groupdet)
                        {
                            dt.Rows.Add(item.intTrnGroupDetail, item.intGroupHeadID, item.BranchCode,
                                        item.intMstDepartment, item.intMstPosition, item.intMstEmpPersonal);
                        }
                    }

                    SqlCommand cmd = new SqlCommand("spSaveGroupTaggings", cons);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@intMstEmpPersonal", GroupHeader.intMstPersonal);
                    cmd.Parameters.AddWithValue("@GroupID", GroupHeader.GroupID);
                    cmd.Parameters.AddWithValue("@GroupName", GroupHeader.GroupName);
                    cmd.Parameters.AddWithValue("@Status", GroupHeader.status);
                    cmd.Parameters.AddWithValue("@Groupdet", dt);

                    cons.Open();


                    result = cmd.ExecuteScalar().ToString();
                    return result;
                }
                catch
                {
                    return result = "";
                }
                finally
                {
                    cons.Close();
                }

               
            }
    
    }

    public class EditTaggings
    {
         SqlConnection cons = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
        string result = "";

            public string editTags(string values, string intMstEmpPersonal, string GroupName, string Status)
            {
              
                try
                {

                    SqlCommand cmd = new SqlCommand("spSaveGroupTagging", cons);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@values", values);
                    cmd.Parameters.AddWithValue("@intMstEmpPersonal", intMstEmpPersonal);
                    cmd.Parameters.AddWithValue("@GroupName", GroupName);
                    cmd.Parameters.AddWithValue("@Status", Status);
                   
                    cons.Open();


                    result = cmd.ExecuteScalar().ToString();
                    return result;
                }
                catch
                {
                    return result = "";
                }
                finally
                {
                    cons.Close();
                }

               
            }

    }

    public class SaveEditTagging
    {
        SqlConnection cons = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
        string result = "";

        public string SaveEditTags(GroupTaggingHeader GroupHeader)
        {

            var dbMgr = new dbManager();    
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnGroupDetail");
            dt.Columns.Add("intGroupHeadID");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");

            try
            {

                if (GroupHeader.Groupdet != null)
                {
                    foreach (var item in GroupHeader.Groupdet)
                    {
                        dt.Rows.Add(item.intTrnGroupDetail, item.intGroupHeadID, item.BranchCode,
                                    item.intMstDepartment, item.intMstPosition, item.intMstEmpPersonal);
                    }
                }


                SqlCommand cmd = new SqlCommand("spSaveGroupTagging", cons);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@intMstEmpPersonal", GroupHeader.intMstPersonal);
                cmd.Parameters.AddWithValue("@GroupID", GroupHeader.GroupID);
                cmd.Parameters.AddWithValue("@GroupName", GroupHeader.GroupName);
                cmd.Parameters.AddWithValue("@Status", GroupHeader.status);
                cmd.Parameters.AddWithValue("@Groupdet", dt);

                //cmd.Parameters.AddWithValue("@values", values);
                //cmd.Parameters.AddWithValue("@intMstEmpPersonal", intMstEmpPersonal);
                //cmd.Parameters.AddWithValue("@GroupID", GroupID);
                //cmd.Parameters.AddWithValue("@GroupName", GroupName);
                //cmd.Parameters.AddWithValue("@Status", Status);

                cons.Open();


                result = cmd.ExecuteScalar().ToString();
                return result;
            }
            catch
            {
                return result = "";
            }
            finally
            {
                cons.Close();
            }


        }
    }

    public class DeleteGroupTags
    {
        SqlConnection cons = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
        string result = "";

        public string DelGroupTags(int ID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spDeleteGroupTags", cons);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", ID);
                
                cons.Open();


                result = cmd.ExecuteScalar().ToString();
                return result;
            }
            catch
            {
                return result = "";
            }
            finally
            {
                cons.Close();
            }


        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class EmployeeNotice
    {
        public int intTrnEmpNotice { get; set; }
        public int intEmpNoticeType { get; set; }
        public string Recipient { get; set; }
        public DateTime EmpNoticeDate { get; set; }
        public DateTime EmpReceivedDate { get; set; }
        public TimeSpan EmpReceivedTime { get; set; }
        public string FileName { get; set; }
        public string OrigFileName { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploadBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string EmpRemarks { get; set; }
        public bool intAutoTag { get; set; }
        public int intDeviationType { get; set; }

        public virtual ICollection<EmployeeNoticeDetail> EmpNoticeDet { get; set; }
    }

    public class EmployeeNoticeDetail
    {
        public int intTrnEmpNoticeDetail { get; set; }
        public int intTrnEmpNotice { get; set; }
        public int intGroupNameID { get; set; }
        public string GroupName { get; set; }
        public int intCompany { get; set; }
        public string Company { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int intMstDepartment { get; set; }
        public string DepartmentName { get; set; }
        public int intMstPosition { get; set; }
        public string PositionName { get; set; }
        public string intMstEmpPersonal { get; set; }
        public string EmpName { get; set; }
        public string intOtherGroup { get; set; }
        public string OtherGroup { get; set; }
    }

    public class EmpNoticeList
    {
        public int intTrnEmpNotice { get; set; }
        public string NoticeTypeName { get; set; }
        public string DeviationType { get; set; }
        public string Description { get; set; }
        public DateTime EmpNoticeDate { get; set; }
        public string FileName { get; set; }
    }

#region DAL
    public static class EmployeeNoticeDAL
    {

        public static string Save(EmployeeNotice not)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnEmpNoticeDetail");
            dt.Columns.Add("intTrnEmpNotice");
            dt.Columns.Add("intGroupNameID");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");
            dt.Columns.Add("intOtherGroup");

            try
            {

                if (not.EmpNoticeDet != null)
                {
                    foreach (var item in not.EmpNoticeDet)
                    {
                        dt.Rows.Add(item.intTrnEmpNoticeDetail, item.intTrnEmpNotice, item.intGroupNameID, item.BranchCode, 
                            item.intMstDepartment, item.intMstPosition, item.intMstEmpPersonal, item.intOtherGroup);
                    }
                }                

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddUploadEmpNotice";
                        cmd.Parameters.Add(new SqlParameter("@intTrnEmpNotice", not.intTrnEmpNotice));
                        cmd.Parameters.Add(new SqlParameter("@intEmpNoticeType", not.intEmpNoticeType));
                        cmd.Parameters.Add(new SqlParameter("@Recipient", not.Recipient));
                        cmd.Parameters.Add(new SqlParameter("@EmpNoticeDate", not.EmpNoticeDate));
                        cmd.Parameters.Add(new SqlParameter("@EmpReceivedDate", not.EmpReceivedDate));
                        cmd.Parameters.Add(new SqlParameter("@EmpReceivedTime", not.EmpReceivedTime));
                        cmd.Parameters.Add(new SqlParameter("@FileName", not.FileName));
                        cmd.Parameters.Add(new SqlParameter("@OrigFileName", not.OrigFileName));
                        cmd.Parameters.Add(new SqlParameter("@Description", not.Description));
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@EmpNoticeDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@EmpRemarks", not.EmpRemarks));
                        cmd.Parameters.Add(new SqlParameter("@intDeviationType", not.intDeviationType));


                        conn.Open();
                        strMsg = (string)cmd.ExecuteScalar();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return strMsg;

        }

        public static string SaveUpload(EmployeeNotice not)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnEmpNoticeDetail");
            dt.Columns.Add("intTrnEmpNotice");
            dt.Columns.Add("intGroupNameID");
            dt.Columns.Add("intCompany");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");
            dt.Columns.Add("intOtherGroup");

            try
            {

                if (not.EmpNoticeDet != null)
                {
                    foreach (var item in not.EmpNoticeDet)
                    {
                        dt.Rows.Add(item.intTrnEmpNoticeDetail, item.intTrnEmpNotice, item.intGroupNameID, item.intCompany, item.BranchCode,
                            item.intMstDepartment, item.intMstPosition, item.intMstEmpPersonal, item.intOtherGroup);
                    }
                }

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddEditEmpNotice";
                        cmd.Parameters.Add(new SqlParameter("@intTrnEmpNotice", not.intTrnEmpNotice));
                        cmd.Parameters.Add(new SqlParameter("@intEmpNoticeType", not.intEmpNoticeType));
                        cmd.Parameters.Add(new SqlParameter("@Recipient", not.Recipient));
                        cmd.Parameters.Add(new SqlParameter("@EmpNoticeDate", not.EmpNoticeDate));
                        cmd.Parameters.Add(new SqlParameter("@EmpReceivedDate", not.EmpReceivedDate));
                        cmd.Parameters.Add(new SqlParameter("@EmpReceivedTime", not.EmpReceivedTime));
                        cmd.Parameters.Add(new SqlParameter("@FileName", not.FileName));
                        cmd.Parameters.Add(new SqlParameter("@OrigFileName", not.OrigFileName));
                        cmd.Parameters.Add(new SqlParameter("@Description", not.Description));
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@EmpNoticeDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@EmpRemarks", not.EmpRemarks));
                        cmd.Parameters.Add(new SqlParameter("@intDeviationType", not.intDeviationType));

                        conn.Open();
                        strMsg = (string)cmd.ExecuteScalar();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return strMsg;
        }



        public static List<EmpNoticeList> GetEmpNoticeList(int intEmpType)
        {
            var dbMgr = new dbManager();
            var list = new List<EmpNoticeList>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_GetItemList";
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@TransType", "Employee Notice"));
                        cmd.Parameters.Add(new SqlParameter("@intEmpType", intEmpType));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var data = new EmpNoticeList()
                                {
                                    intTrnEmpNotice = Convert.ToInt32(rdr[0]),
                                    NoticeTypeName = rdr[10].ToString(),
                                    DeviationType = rdr[11].ToString(),
                                    EmpNoticeDate = Convert.ToDateTime(rdr[2]),                                    
                                    Description = rdr[5].ToString(),
                                    FileName = rdr[3].ToString(),
                                };

                                list.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return list;
        }

        public static EmployeeNotice GetEmpNotice(int intTrnEmpNotice)
        {
            var dbMgr = new dbManager();
            var data = new EmployeeNotice();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "SELECT * FROM tblTrnEmpNotice WHERE (intTrnEmpNotice = @intTrnEmpNotice)";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetEmpNoticeHeader";
                        cmd.Parameters.Add(new SqlParameter("@intTrnEmpNotice", intTrnEmpNotice));                    
                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {

                                data.intTrnEmpNotice = Convert.ToInt32(rdr[0]);
                                data.intEmpNoticeType = Convert.ToInt32(rdr[1]);
                                data.EmpNoticeDate = Convert.ToDateTime(rdr[2]);
                                data.EmpReceivedDate = Convert.ToDateTime(rdr[3]);
                                data.EmpReceivedTime = TimeSpan.Parse(rdr[4].ToString());
                                data.FileName = rdr[5].ToString();
                                data.OrigFileName = rdr[6].ToString();
                                data.Description = rdr[7].ToString();
                                data.Recipient = rdr[8].ToString();
                                data.EmpRemarks = rdr[9].ToString();
                                data.intDeviationType = Convert.ToInt32(rdr[10]);
                            }

                            data.EmpNoticeDet = GetEmpNoticeDetail(intTrnEmpNotice);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }

            return data;
        }

        public static List<EmployeeNoticeDetail> GetEmpNoticeDetail(int intTrnEmpNotice)
        {
            var dbMgr = new dbManager();
            var list = new List<EmployeeNoticeDetail>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetEmpNoticeDetail";
                        cmd.Parameters.Add(new SqlParameter("@intTrnEmpNotice", intTrnEmpNotice));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var data = new EmployeeNoticeDetail()
                                {
                                    intTrnEmpNoticeDetail = Convert.ToInt32(rdr[0]),
                                    intTrnEmpNotice = Convert.ToInt32(rdr[1]),
                                    intGroupNameID = Convert.ToInt32(rdr[2]),
                                    GroupName = rdr[3].ToString(),
                                    intCompany = Convert.ToInt32(rdr[4]),
                                    Company = rdr[5].ToString(),
                                    BranchCode = rdr[6].ToString(),
                                    BranchName = rdr[7].ToString(),
                                    intMstDepartment = Convert.ToInt32(rdr[8]),
                                    DepartmentName = rdr[9].ToString(),
                                    intMstPosition = Convert.ToInt32(rdr[10]),
                                    PositionName = rdr[11].ToString(),
                                    intMstEmpPersonal = rdr[12].ToString(),
                                    EmpName = rdr[13].ToString(),
                                    intOtherGroup = rdr[14].ToString(),
                                    OtherGroup = rdr[15].ToString()

                                };

                                list.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return list;
        }

        public static List<ComboBoxSource> GetNoticeType()
        {
            string query = string.Empty;

            query = "SELECT * FROM tblMstNoticeType WHERE intMstNoticeType NOT IN(6) ORDER BY NoticeTypeName";

            return OthersDAL.GetDataCombo(query, "Connection");
        }

        public static List<ComboBoxSource> GetAuditType()
        {
            string query = string.Empty;

            query = "SELECT * FROM tblMstNoticeType WHERE intMstNoticeType = 6 ORDER BY NoticeTypeName";

            return OthersDAL.GetDataCombo(query, "Connection");
        }

        public static List<ComboBoxSource> GetDeviationType()
        {
            string query = string.Empty;

            query = "SELECT * FROM tblMstDeviationType ORDER BY ID";

            return OthersDAL.GetDataCombo(query, "Connection");
        }

        public static List<ComboBoxSource> GetComboEmpNoticeList(bool haveEmptyString = false)
        {
            var dataResult = new List<ComboBoxSource>();
            string query = string.Empty;

            if (haveEmptyString == true)
            {
                query = "SELECT '' AS [FileName],' ALL' AS [Description] UNION ";
            }

            query += "SELECT [FileName], [Description] FROM tblTrnEmpNotice ORDER BY [Description]";

            return OthersDAL.GetDataCombo(query, "Connection");
        }

        public static List<ComboBoxSource> GetUsers(string fileName)
        {
            var dataResult = new List<ComboBoxSource>();
            string query = string.Empty;

            query = "Others_GetUserEmpNamefromHRIS '" + fileName + "'";
            return OthersDAL.GetDataCombo(query, "Connection");
        }

    }
#endregion

}
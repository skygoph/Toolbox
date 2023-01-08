using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class Policy
    {
        public int intTrnPolicy { get; set; }
        public string DocNo { get; set; }
        public string Recipient { get; set; }
        public string PolicyTitle { get; set; }
        public DateTime DateEffective { get; set; }
        public DateTime PolDateReceived { get; set; }
        public TimeSpan PolTimeReceived { get; set; }
        public int PolicyType { get; set; }
        public string PolicyDescription { get; set; }
        public string FileName { get; set; }
        public string OrigFileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploadBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int intCounter { get; set; }
        public bool isSecondSave { get; set; }
        public string PolicyRemarks { get; set; }
        public bool intAutoTag { get; set; }

        public virtual ICollection<PolicyDetail> PolicyDet { get; set; }
    }

    public class PolicyDetail
    {
        public int intTrnPolicyDetail { get; set; }
        public int intTrnPolicy { get; set; }
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


#region DAL
    public static class PolicyDAL
    {
        public static string Save(Policy policy)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnPolicyDetail");
            dt.Columns.Add("intTrnPolicy");
            dt.Columns.Add("intGroupNameID");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");
            dt.Columns.Add("intOtherGroup");

            try
            {
                if (policy.PolicyDet != null)
                {
                    foreach (var item in policy.PolicyDet)
                    {
                        dt.Rows.Add(item.intTrnPolicyDetail, item.intTrnPolicy, item.intGroupNameID, item.BranchCode, item.intMstDepartment,
                                    item.intMstPosition, item.intMstEmpPersonal, item.intOtherGroup);
                    }
                }

                if (policy.DocNo != "ERROR")
                { 
                    string[] items = policy.DocNo.Split(' ');
                    policy.intCounter = Convert.ToInt32(items[2]);
                }

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddUploadPolicy";
                        cmd.Parameters.Add(new SqlParameter("@intTrnPolicy", policy.intTrnPolicy));
                        cmd.Parameters.Add(new SqlParameter("@DocNo", policy.DocNo));
                        cmd.Parameters.Add(new SqlParameter("@Recipient", policy.Recipient));
                        cmd.Parameters.Add(new SqlParameter("@DateEffective", policy.DateEffective));
                        cmd.Parameters.Add(new SqlParameter("@PolDateReceived", policy.PolDateReceived));
                        cmd.Parameters.Add(new SqlParameter("@PolTimeReceived", policy.PolTimeReceived));
                        cmd.Parameters.Add(new SqlParameter("@PolicyTitle", policy.PolicyTitle));
                        cmd.Parameters.Add(new SqlParameter("@PolicyType", policy.PolicyType));
                        cmd.Parameters.Add(new SqlParameter("@PolicyDescription", policy.PolicyDescription));
                        cmd.Parameters.Add(new SqlParameter("@FileName", policy.FileName));
                        cmd.Parameters.Add(new SqlParameter("@OrigFileName", policy.OrigFileName));
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@PolicyDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@intCounter", policy.intCounter));
                        cmd.Parameters.Add(new SqlParameter("@isSecondSave", policy.isSecondSave));
                        cmd.Parameters.Add(new SqlParameter("@PolicyRemarks", policy.PolicyRemarks));
                        cmd.Parameters.Add(new SqlParameter("@intAutoTag", policy.intAutoTag));

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

        public static string UploadSave(Policy policy)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnPolicyDetail");
            dt.Columns.Add("intTrnPolicy");
            dt.Columns.Add("intGroupNameID");
            dt.Columns.Add("intCompany");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");
            dt.Columns.Add("intOtherGroup");

            try
            {
                if (policy.PolicyDet != null)
                {
                    foreach (var item in policy.PolicyDet)
                    {
                        dt.Rows.Add(item.intTrnPolicyDetail, item.intTrnPolicy, item.intGroupNameID, item.intCompany, item.BranchCode, item.intMstDepartment,
                                    item.intMstPosition, item.intMstEmpPersonal, item.intOtherGroup);
                    }
                }

                if (policy.DocNo != "ERROR")
                {
                    string[] items = policy.DocNo.Split(' ');
                    policy.intCounter = Convert.ToInt32(items[2]);
                }

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddEditPolicy";
                        cmd.Parameters.Add(new SqlParameter("@intTrnPolicy", policy.intTrnPolicy));
                        cmd.Parameters.Add(new SqlParameter("@DocNo", policy.DocNo));
                        cmd.Parameters.Add(new SqlParameter("@Recipient", policy.Recipient));
                        cmd.Parameters.Add(new SqlParameter("@DateEffective", policy.DateEffective));
                        cmd.Parameters.Add(new SqlParameter("@PolDateReceived", policy.PolDateReceived));
                        cmd.Parameters.Add(new SqlParameter("@PolTimeReceived", policy.PolTimeReceived));
                        cmd.Parameters.Add(new SqlParameter("@PolicyTitle", policy.PolicyTitle));
                        cmd.Parameters.Add(new SqlParameter("@PolicyType", policy.PolicyType));
                        cmd.Parameters.Add(new SqlParameter("@PolicyDescription", policy.PolicyDescription));
                        cmd.Parameters.Add(new SqlParameter("@FileName", policy.FileName));
                        cmd.Parameters.Add(new SqlParameter("@OrigFileName", policy.OrigFileName));
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@PolicyDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@intCounter", policy.intCounter));
                        cmd.Parameters.Add(new SqlParameter("@isSecondSave", policy.isSecondSave));
                        cmd.Parameters.Add(new SqlParameter("@PolicyRemarks", policy.PolicyRemarks));
                        cmd.Parameters.Add(new SqlParameter("@intAutoTag", policy.intAutoTag));

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


        public static List<Policy> GetPolicyList()
        {
            var dbMgr = new dbManager();
            var list = new List<Policy>();

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
                        cmd.Parameters.Add(new SqlParameter("@TransType", "Policy"));
                      

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                var data = new Policy
                                {
                                    intTrnPolicy = Convert.ToInt32(rdr[0]),
                                    DocNo = rdr[1].ToString(),
                                    PolicyTitle = rdr[2].ToString(),
                                    DateEffective = Convert.ToDateTime(rdr[3]),
                                    PolicyType = Convert.ToInt32(rdr[4]),
                                    PolicyDescription = rdr[5].ToString(),
                                    FileName = rdr[6].ToString(),
                                    OrigFileName = rdr[7].ToString(),
                                    UploadDate = Convert.ToDateTime(rdr[8])
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

        public static Policy GetPolicy(int intTrnPolicy)
        {
            var dbMgr = new dbManager();
            var data = new Policy();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        //cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "SELECT * FROM tblTrnPolicy WHERE (intTrnPolicy = @intTrnPolicy)";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetPolicyHeader";
                        cmd.Parameters.Add(new SqlParameter("@intTrnPolicy", intTrnPolicy));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                data.intTrnPolicy = Convert.ToInt32(rdr[0]);
                                data.DocNo = rdr[1].ToString();
                                data.PolicyTitle = rdr[2].ToString();
                                data.DateEffective = Convert.ToDateTime(rdr[3]);
                                data.PolDateReceived = Convert.ToDateTime(rdr[4]);
                                data.PolTimeReceived = TimeSpan.Parse(rdr[5].ToString());
                                data.PolicyType = Convert.ToInt32(rdr[6]);
                                data.PolicyDescription = rdr[7].ToString();
                                data.FileName = rdr[8].ToString();
                                data.OrigFileName = rdr[9].ToString();
                                data.Recipient = rdr[10].ToString();
                                data.isSecondSave = false;
                                data.PolicyRemarks = rdr[11].ToString();
                                data.intAutoTag = Convert.ToBoolean(rdr[12]);
                            }

                            data.PolicyDet = GetPolicyDetail(intTrnPolicy);
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

        public static List<PolicyDetail> GetPolicyDetail(int intTrnPolicy)
        {
            var dbMgr = new dbManager();
            var data = new List<PolicyDetail>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetPolicyDetail";
                        cmd.Parameters.Add(new SqlParameter("@intTrnPolicy", intTrnPolicy));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new PolicyDetail
                                {
                                    intTrnPolicyDetail = Convert.ToInt32(rdr[0]),
                                    intTrnPolicy = Convert.ToInt32(rdr[1]),
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

                                data.Add(item);
                            }
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

        public static List<ComboBoxSource> GetPolicyType()
        {
            string query = string.Empty;

            query = "SELECT * FROM tblMstPolicyType ORDER BY PolicyTypeName";

            return OthersDAL.GetDataCombo(query, "Connection");
        }

        public static string GetLastMemoNo()
        {
            return Utilities.GetAutoGenerateLastNo("Memo");
        }

        public static List<ComboBoxSource> GetComboPolicyList(bool haveEmptyString = false)
        {
            var dataResult = new List<ComboBoxSource>();
            string query = string.Empty;

            if (haveEmptyString == true)
            {
                query = "SELECT '' AS [FileName],' ALL' AS PolicyTitle UNION ";
            }

            query += "SELECT [FileName], PolicyTitle FROM tblTrnPolicy ORDER BY PolicyTitle";

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
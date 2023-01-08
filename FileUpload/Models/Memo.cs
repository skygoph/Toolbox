using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class Memo
    {
        public int intTrnMemo { get; set; }
        public string DocNo { get; set; }
        public string Subject { get; set; }
        public string MemoDescription { get; set; }
        public string MemoRemarks { get; set; }
        public string Recipient { get; set; }
        public DateTime MemoDate { get; set; }
        public DateTime MemoReceivedDate { get; set; }
        public TimeSpan MemoReceivedTime { get; set; }
        public string FileName { get; set; }
        public string OrigFileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploadBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string PrefixName { get; set; }
        public int intCounter { get; set; }
        public int intYear { get; set; }
        public bool isSecondSave { get; set; }
        public int intMstDepartment { get; set; }
        public bool intAutoTag { get; set; }
  

        public virtual ICollection<MemoDetail> MemoDet { get; set; }
    }

    public class MemoDetail
    {
        public int intTrnMemoDetail { get; set; }
        public int intTrnMemo { get; set; }
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

    public static class MemoDAL
    {        

        public static string Save(Memo memo)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnMemoDetail");
            dt.Columns.Add("intTrnMemo");
            dt.Columns.Add("intGroupNameID");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");
            dt.Columns.Add("intOtherGroup");

            try
            {

                if (memo.MemoDet != null)
                {
                    foreach (var item in memo.MemoDet)
                    {
                        dt.Rows.Add(item.intTrnMemoDetail, item.intTrnMemo, item.intGroupNameID, item.BranchCode,
                                    item.intMstDepartment, item.intMstPosition, item.intMstEmpPersonal, item.intOtherGroup);
                    }
                }

                if (memo.DocNo != "ERROR")
                {
                    string[] items = memo.DocNo.Split('-');
                    memo.PrefixName = items[0];
                    memo.intCounter = Convert.ToInt32(items[1]);
                    memo.intYear = Convert.ToInt32(items[2]);
                }

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddUploadMemo";
                        cmd.Parameters.Add(new SqlParameter("@intTrnMemo", memo.intTrnMemo));
                        cmd.Parameters.Add(new SqlParameter("@DocNo", memo.DocNo));
                        cmd.Parameters.Add(new SqlParameter("@Subject", memo.Subject));
                        cmd.Parameters.Add(new SqlParameter("@Recipient", memo.Recipient));
                        cmd.Parameters.Add(new SqlParameter("@MemoDescription", memo.MemoDescription));
                        cmd.Parameters.Add(new SqlParameter("@MemoDate", Convert.ToDateTime(memo.MemoDate)));
                        cmd.Parameters.Add(new SqlParameter("@MemoReceivedDate", Convert.ToDateTime(memo.MemoReceivedDate)));
                        cmd.Parameters.Add(new SqlParameter("@MemoReceivedTime", memo.MemoReceivedTime));
                        cmd.Parameters.Add(new SqlParameter("@FileName", memo.FileName));
                        cmd.Parameters.Add(new SqlParameter("@OrigFileName", memo.OrigFileName));
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@MemoDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@PrefixName", memo.PrefixName));
                        cmd.Parameters.Add(new SqlParameter("@intCounter", memo.intCounter));
                        cmd.Parameters.Add(new SqlParameter("@intYear", memo.intYear));
                        cmd.Parameters.Add(new SqlParameter("@isSecondSave", memo.isSecondSave));
                        cmd.Parameters.Add(new SqlParameter("@intMstDepartment", memo.intMstDepartment));
                        cmd.Parameters.Add(new SqlParameter("@MemoRemarks", memo.MemoRemarks));
                        cmd.Parameters.Add(new SqlParameter("@intAutoTag", memo.intAutoTag));

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

        public static string SaveUpload(Memo memo)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnMemoDetail");
            dt.Columns.Add("intTrnMemo");
            dt.Columns.Add("intGroupNameID");
            dt.Columns.Add("intCompany");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");
            dt.Columns.Add("intOtherGroup");

            try
            {

                if (memo.MemoDet != null)
                {
                    foreach (var item in memo.MemoDet)
                    {
                        dt.Rows.Add(item.intTrnMemoDetail, item.intTrnMemo, item.intGroupNameID, item.intCompany, item.BranchCode,
                                    item.intMstDepartment, item.intMstPosition, item.intMstEmpPersonal, item.intOtherGroup);
                    }
                }

                if (memo.DocNo != "ERROR")
                {
                    string[] items = memo.DocNo.Split('-');
                    memo.PrefixName = items[0];
                    memo.intCounter = Convert.ToInt32(items[1]);
                    memo.intYear = Convert.ToInt32(items[2]);
                }

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddEditMemo";
                        cmd.Parameters.Add(new SqlParameter("@intTrnMemo", memo.intTrnMemo));
                        cmd.Parameters.Add(new SqlParameter("@DocNo", memo.DocNo));
                        cmd.Parameters.Add(new SqlParameter("@Subject", memo.Subject));
                        cmd.Parameters.Add(new SqlParameter("@Recipient", memo.Recipient));
                        cmd.Parameters.Add(new SqlParameter("@MemoDescription", memo.MemoDescription));
                        cmd.Parameters.Add(new SqlParameter("@MemoDate", Convert.ToDateTime(memo.MemoDate)));
                        cmd.Parameters.Add(new SqlParameter("@MemoReceivedDate", Convert.ToDateTime(memo.MemoReceivedDate)));
                        cmd.Parameters.Add(new SqlParameter("@MemoReceivedTime", memo.MemoReceivedTime));
                        cmd.Parameters.Add(new SqlParameter("@FileName", memo.FileName));
                        cmd.Parameters.Add(new SqlParameter("@OrigFileName", memo.OrigFileName));
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@MemoDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@PrefixName", memo.PrefixName));
                        cmd.Parameters.Add(new SqlParameter("@intCounter", memo.intCounter));
                        cmd.Parameters.Add(new SqlParameter("@intYear", memo.intYear));
                        cmd.Parameters.Add(new SqlParameter("@isSecondSave", memo.isSecondSave));
                        cmd.Parameters.Add(new SqlParameter("@intMstDepartment", memo.intMstDepartment));
                        cmd.Parameters.Add(new SqlParameter("@MemoRemarks", memo.MemoRemarks));
                        cmd.Parameters.Add(new SqlParameter("@intAutoTag", memo.intAutoTag));

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

        public static List<Memo> GetMemoList()
        {
            var dbMgr = new dbManager();
            var list = new List<Memo>();

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
                        cmd.Parameters.Add(new SqlParameter("@TransType", "Memorandum"));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                var data = new Memo
                                {
                                    intTrnMemo = Convert.ToInt32(rdr[0]),
                                    DocNo = rdr[1].ToString(),
                                    Subject = rdr[2].ToString(),
                                    MemoDate = Convert.ToDateTime(rdr[3]),
                                    MemoDescription = rdr[4].ToString(),                                    
                                    FileName = rdr[5].ToString(),
                                    OrigFileName = rdr[6].ToString(),
                                    UploadDate = Convert.ToDateTime(rdr[7])
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

        public static List<MemoDetail> GetMemoDetail(int intTrnMemo)
        {
            var dbMgr = new dbManager();
            var data = new List<MemoDetail>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetMemoDetail";
                        cmd.Parameters.Add(new SqlParameter("@intTrnMemo", intTrnMemo));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new MemoDetail 
                                { 
                                    intTrnMemoDetail = Convert.ToInt32(rdr[0]),
                                    intTrnMemo = Convert.ToInt32(rdr[1]),
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

        public static Memo GetMemo(int intTrnMemo)
        {
            var dbMgr = new dbManager();
            var data = new Memo();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        //cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "SELECT * FROM tblTrnMemo WHERE (intTrnMemo = @intTrnMemo)";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetMemoHeader";
                        cmd.Parameters.Add(new SqlParameter("@intTrnMemo", intTrnMemo));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            { 
                                data.intTrnMemo = Convert.ToInt32(rdr[0]);
                                data.DocNo = rdr[1].ToString();
                                data.Subject = rdr[2].ToString();
                                data.MemoDate = Convert.ToDateTime(rdr[3]);
                                data.MemoReceivedDate = Convert.ToDateTime(rdr[4]);
                                data.MemoReceivedTime = TimeSpan.Parse(rdr[5].ToString());
                                data.MemoDescription = rdr[6].ToString();                                
                                data.FileName = rdr[7].ToString();
                                data.OrigFileName = rdr[8].ToString();
                                data.Recipient = rdr[9].ToString();
                                data.isSecondSave = false;
                                data.MemoRemarks = rdr[10].ToString();
                                data.intAutoTag = Convert.ToBoolean(rdr[11]);
                            }

                            data.MemoDet = GetMemoDetail(intTrnMemo);
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

        public static List<ViewSendTo> GetSentTo()
        {
            var dbMgr = new dbManager();
            var list = new List<ViewSendTo>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT CategoryName FROM tblMstSendTo ORDER BY CategoryName";

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                var data = new ViewSendTo
                                {                                    
                                    CategoryName = rdr[0].ToString()
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

        public static List<ComboBoxSource> GetmemoData(string codeMstBranch, int intMstDept, int intMstPosition, string LookFor)
        {

            var dataResult = new List<ComboBoxSource>();
            string query = string.Empty;

            //codeMstBranch = codeMstBranch == "" ? "%" : codeMstBranch;
            //intMstDept = intMstDept;
            //intMstPosition = intMstPosition;

            query = "[Toolbox_GetData] '" + codeMstBranch + "', " + intMstDept + ", " + intMstPosition + ", '" + LookFor + "'";
            return OthersDAL.GetDataCombo(query);

        }

        public static List<ComboBoxSource> GetComboMemoList(bool haveEmptyString = false)
        {
            var dataResult = new List<ComboBoxSource>();
            string query = string.Empty;

            if (haveEmptyString == true){
                query = "SELECT '' AS [FileName],' ALL' AS [Subject] UNION ";
            }

            query += "SELECT [FileName], [Subject] FROM tblTrnMemo ORDER BY [Subject]";

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
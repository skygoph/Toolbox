using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class Notification
    {
        public int intTrnNotification { get; set; }
        public string DocNo { get; set; }
        public DateTime NotiReceivedDate { get; set; }
        public TimeSpan NotiReceivedTime { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string FileName { get; set; }
        public string OrigFileName { get; set; }
        public string NotiDescription { get; set; }
        public string NotiRemarks { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploadBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int intCounter { get; set; }
        public bool isSecondSave { get; set; }
        public bool intAutoTag { get; set; } 

        public virtual ICollection<NotificationDetail> NotificationDet { get; set; }
    }

    public class NotificationDetail
    {
        public int intTrnNotificationDetail { get; set; }
        public int intTrnNotification { get; set; }
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
    public static class NotificationDAL
    {

        public static string Save(Notification noti)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnNotificationDetail");
            dt.Columns.Add("intTrnNotification");
            dt.Columns.Add("intGroupNameID");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");
            dt.Columns.Add("intOtherGroup");

            try
            {
                if (noti.NotificationDet != null)
                {
                    foreach (var item in noti.NotificationDet)
                    {
                        dt.Rows.Add(item.intTrnNotificationDetail, item.intTrnNotification,item.intGroupNameID ,item.BranchCode,
                                    item.intMstDepartment, item.intMstPosition, item.intMstEmpPersonal, item.intOtherGroup);
                    }
                }

                if (noti.DocNo != "ERROR")
                {
                    string[] items = noti.DocNo.Split('-');
                    noti.intCounter = Convert.ToInt32(items[1]);
                }

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddUploadNotification";
                        cmd.Parameters.Add(new SqlParameter("@intTrnNotification", noti.intTrnNotification));
                        cmd.Parameters.Add(new SqlParameter("@DocNo", noti.DocNo));
                        cmd.Parameters.Add(new SqlParameter("@Recipient", noti.Recipient));
                        cmd.Parameters.Add(new SqlParameter("@NotiReceivedDate", Convert.ToDateTime(noti.NotiReceivedDate)));
                        cmd.Parameters.Add(new SqlParameter("@NotiReceivedTime", noti.NotiReceivedTime));
                        cmd.Parameters.Add(new SqlParameter("@Subject", noti.Subject));
                        cmd.Parameters.Add(new SqlParameter("@FileName", noti.FileName));
                        cmd.Parameters.Add(new SqlParameter("@OrigFileName", noti.OrigFileName));
                        cmd.Parameters.Add(new SqlParameter("@NotiDescription", noti.NotiDescription));
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@NotificationDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@intCounter", noti.intCounter));
                        cmd.Parameters.Add(new SqlParameter("@isSecondSave", noti.isSecondSave));
                        cmd.Parameters.Add(new SqlParameter("@NotiRemarks", noti.NotiRemarks));
                        cmd.Parameters.Add(new SqlParameter("@intAutoTag", noti.intAutoTag));

                        conn.Open();
                        strMsg=(string)cmd.ExecuteScalar();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return strMsg;

        }

        public static string UploadSave(Notification noti)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;
            DataTable dt = new DataTable();

            dt.Columns.Add("intTrnNotificationDetail");
            dt.Columns.Add("intTrnNotification");
            dt.Columns.Add("intGroupNameID");
            dt.Columns.Add("intCompany");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("intMstDepartment");
            dt.Columns.Add("intMstPosition");
            dt.Columns.Add("intMstEmpPersonal");
            dt.Columns.Add("intOtherGroup");

            try
            {
                if (noti.NotificationDet != null)
                {
                    foreach (var item in noti.NotificationDet)
                    {
                        dt.Rows.Add(item.intTrnNotificationDetail, item.intTrnNotification, item.intGroupNameID, item.intCompany, item.BranchCode,
                                    item.intMstDepartment, item.intMstPosition, item.intMstEmpPersonal, item.intOtherGroup);
                    }
                }

                if (noti.DocNo != "ERROR")
                {
                    string[] items = noti.DocNo.Split('-');
                    noti.intCounter = Convert.ToInt32(items[1]);
                }

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;   
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddEditNotification";
                        cmd.Parameters.Add(new SqlParameter("@intTrnNotification", noti.intTrnNotification));
                        cmd.Parameters.Add(new SqlParameter("@DocNo", noti.DocNo));
                        cmd.Parameters.Add(new SqlParameter("@Recipient", noti.Recipient));
                        cmd.Parameters.Add(new SqlParameter("@NotiReceivedDate", Convert.ToDateTime(noti.NotiReceivedDate)));
                        cmd.Parameters.Add(new SqlParameter("@NotiReceivedTime", noti.NotiReceivedTime));
                        cmd.Parameters.Add(new SqlParameter("@Subject", noti.Subject));
                        cmd.Parameters.Add(new SqlParameter("@FileName", noti.FileName));
                        cmd.Parameters.Add(new SqlParameter("@OrigFileName", noti.OrigFileName));
                        cmd.Parameters.Add(new SqlParameter("@NotiDescription", noti.NotiDescription));
                        cmd.Parameters.Add(new SqlParameter("@Username", HttpContext.Current.Session["Username"]));
                        cmd.Parameters.Add(new SqlParameter("@NotificationDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@intCounter", noti.intCounter));
                        cmd.Parameters.Add(new SqlParameter("@isSecondSave", noti.isSecondSave));
                        cmd.Parameters.Add(new SqlParameter("@NotiRemarks", noti.NotiRemarks));
                        cmd.Parameters.Add(new SqlParameter("@intAutoTag", noti.intAutoTag));

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

        public static List<Notification> GetNotificationList()
        {
            var dbMgr = new dbManager();
            var list = new List<Notification>();

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
                        cmd.Parameters.Add(new SqlParameter("@TransType", "Notification"));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var data = new Notification() 
                                { 
                                    intTrnNotification = Convert.ToInt32(rdr[0]),
                                    DocNo = rdr[1].ToString(),
                                    Subject = rdr[2].ToString(),
                                    FileName = rdr[3].ToString(),
                                    OrigFileName = rdr[4].ToString(),
                                    NotiDescription = rdr[5].ToString()
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

        public static Notification GetNotification(int intTrnNotification)
        {
            var dbMgr = new dbManager();
            var data = new Notification();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        //cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "SELECT intTrnNotification, DocNo, CAST(NotiReceivedDate as DATE), CAST(NotiReceivedTime as TIME(0), Subject, FileName, OrigFileName, NotiDescription,) FROM tblTrnNotification WHERE (intTrnNotification = @intTrnNotification)";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetNotificationHeader";
                        cmd.Parameters.Add(new SqlParameter("@intTrnNotification", intTrnNotification));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                
                                data.intTrnNotification = Convert.ToInt32(rdr[0]);
                                data.DocNo = rdr[1].ToString();
                                data.NotiReceivedDate = Convert.ToDateTime(rdr[2].ToString());
                                data.NotiReceivedTime = TimeSpan.Parse(rdr[3].ToString());
                                data.Subject = rdr[4].ToString();
                                data.FileName = rdr[5].ToString();
                                data.OrigFileName = rdr[6].ToString();
                                data.NotiDescription = rdr[7].ToString();
                                data.Recipient = rdr[8].ToString();
                                data.NotiRemarks = rdr[9].ToString();
                                data.isSecondSave = false;
                                data.intAutoTag = Convert.ToBoolean(rdr[10]);
                            }

                            data.NotificationDet = GetNotificationDetail(intTrnNotification);
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

        public static List<NotificationDetail> GetNotificationDetail(int intTrnNotification)
        {
            var dbMgr = new dbManager();
            var list = new List<NotificationDetail>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetNotificationDetail";
                        cmd.Parameters.Add(new SqlParameter("@intTrnNotification", intTrnNotification));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var data = new NotificationDetail() 
                                {
                                    intTrnNotificationDetail = Convert.ToInt32(rdr[0]),
                                    intTrnNotification = Convert.ToInt32(rdr[1]),
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

        public static string GetLastNotificationNo()
        {
            return Utilities.GetAutoGenerateLastNo("Announcement");
        }

        public static List<ComboBoxSource> GetComboNotificationList(bool haveEmptyString = false)
        {
            var dataResult = new List<ComboBoxSource>();
            string query = string.Empty;

            if (haveEmptyString == true)
            {
                query = "SELECT '' AS [FileName],' ALL' AS [Subject] UNION ";
            }

            query += "SELECT [FileName], [Subject] FROM tblTrnNotification WHERE [Subject] <> '' ORDER BY [Subject]";

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
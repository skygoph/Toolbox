using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class NasiAnnouncement
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

        public virtual ICollection<NasiNotificationDetail> NasiNotificationDet { get; set; }
    }

    public class NasiNotificationDetail
    {
        public int intTrnNotificationDetail { get; set; }
        public int intTrnNotification { get; set; }
        public int intGroupNameID { get; set; }
        public string GroupName { get; set; }
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


    public static class NasiNotificationDAL
    {

        public static string Save(NasiAnnouncement noti)
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
                if (noti.NasiNotificationDet != null)
                {
                    foreach (var item in noti.NasiNotificationDet)
                    {
                        dt.Rows.Add(item.intTrnNotificationDetail, item.intTrnNotification, item.intGroupNameID, item.BranchCode,
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
                        cmd.CommandText = "Trn_AddUploadNasiNotification";
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
                        cmd.Parameters.Add(new SqlParameter("@NasiNotificationDet", dt));
                        cmd.Parameters.Add(new SqlParameter("@intCounter", noti.intCounter));
                        cmd.Parameters.Add(new SqlParameter("@isSecondSave", noti.isSecondSave));
                        cmd.Parameters.Add(new SqlParameter("@NotiRemarks", noti.NotiRemarks));

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
    } //end of DAL
}
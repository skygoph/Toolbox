using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class Download
    {
    }

    public class DownloadCounter
    {
        public int intTrnDownloadCounter { get; set; }
        public int CategoryID { get; set; }
        public string FileName { get; set; }
        public string DownloadedBy { get; set; }
        public string DownloadedDate { get; set; }
    }

    #region DAL
    public class DownloadDAL
    {
        public static string AddCount(DownloadCounter dlCounter)
        {
            var dbMgr = new dbManager();
            string _result = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[Trn_Counter]";
                        //cmd.Parameters.Add(new SqlParameter("@intTrnDownloadCounter", dlCounter.intTrnDownloadCounter));
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", dlCounter.CategoryID));
                        cmd.Parameters.Add(new SqlParameter("@FileName", dlCounter.FileName));
                        cmd.Parameters.Add(new SqlParameter("@DownloadedBy", dlCounter.DownloadedBy));
                        cmd.Parameters.Add(new SqlParameter("@DownloadedDate", DateTime.Now));

                        conn.Open();
                        _result = (string)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());                
            }

            return _result;
        }
    }
    #endregion
}
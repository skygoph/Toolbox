using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace FileUpload.Models
{
    public class AudienceMonitoringReport
    {
        public string Username { get; set; }
        public string Subject { get; set; }
        public DateTime DateUploaded { get; set; }
        public string DocNo { get; set; }
    }

    public class ReportsDAL
    {
        //Audience Monitoring Detailed Report
        public DataTable GetAMDReport(string fileName, string userName, string moduleName)
        {
            var dbMgr = new dbManager();
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Report_AudienceMonitoringDetailed";
                        cmd.Parameters.Add(new SqlParameter("@Subject", fileName));
                        cmd.Parameters.Add(new SqlParameter("@Username", userName));
                        cmd.Parameters.Add(new SqlParameter("@Module", moduleName));

                        conn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dt.Clear();
                            adp.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        //Audience Monitoring Summary
        public DataTable GetAMSReport(string fileName, DateTime AsofDate, string moduleName)
        {
            var dbMgr = new dbManager();
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Report_AudienceMonitoringSummary";
                        cmd.Parameters.Add(new SqlParameter("@Subject", fileName));
                        cmd.Parameters.Add(new SqlParameter("@AsofDate", AsofDate));
                        cmd.Parameters.Add(new SqlParameter("@Module", moduleName));

                        conn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dt.Clear();
                            adp.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        //Download Trail
        public DataTable GetDownloadTrail(string fileName, string userName, string moduleName)
        {
            var dbMgr = new dbManager();
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Report_DownloadTrail";
                        cmd.Parameters.Add(new SqlParameter("@Subject", fileName));
                        cmd.Parameters.Add(new SqlParameter("@Username", userName));
                        cmd.Parameters.Add(new SqlParameter("@Module", moduleName));

                        conn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dt.Clear();
                            adp.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }


        public DataTable GetUploadTaggings(int ID, string moduleName)
        {
            var dbMgr = new dbManager();
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "spGetUploadTaggings";
                        cmd.Parameters.Add(new SqlParameter("@ID", ID));
                        cmd.Parameters.Add(new SqlParameter("@DocType", moduleName));                   

                        conn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dt.Clear();
                            adp.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }

        public DataTable GetUploadFilesSummaryReport(string userID, string Type, DateTime DateFrom, DateTime DateTo)
        {
            var dbMgr = new dbManager();
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Report_GetUploadFilesSummaryReport";
                        cmd.Parameters.Add(new SqlParameter("@userID", userID));
                        cmd.Parameters.Add(new SqlParameter("@strType", Type));
                        cmd.Parameters.Add(new SqlParameter("@dateFrom", DateFrom));
                        cmd.Parameters.Add(new SqlParameter("@dateTo", DateTo));

                        conn.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            dt.Clear();
                            adp.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception)
            {
                dt = null;
            }

            return dt;
        }



    }
}
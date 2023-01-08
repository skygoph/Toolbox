using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class Upload
    {
        public int intTrnUpload { get; set; }
        public int CategoryID { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string Subject { get; set; }
        public int intMstGroupType { get; set; }
        public int intMstDept { get; set; }
        public string FileSize { get; set; }
        public DateTime dataDate { get; set; }
        public string newFileName { get; set; }
        public bool isOverride { get; set; }
        public string OverrideBy { get; set; }        
    }

    public class UploadList
    {
        public int intTrnUpload { get; set; }
        public int CategoryID { get; set; }
        public int intMstGroupType { get; set; }
        public int intMstDept { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string dataDate { get; set; }
        public string Subject { get; set; }
        public string FileSize { get; set; }
        public string GroupType { get; set; }
        public string DeptName { get; set; }        
    }

    public class UploadSetCategory
    {
        public virtual ICollection<Upload> uplFiles { get; set; }
        public virtual ICollection<Category> catList { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public DateTime dataDate { get; set; }
        public string Subject { get; set; }
        public int intMstGroupType { get; set; }
        public int intMstDept { get; set; }
        public string FileSize { get; set; }
    }

    #region DAL
    public static class UploadDAL
    {
        public static string Save(Upload upl, DataTable dt, string username)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_AddEditUpload";
                        cmd.Parameters.Add(new SqlParameter("@intTrnUpload", upl.intTrnUpload));
                        cmd.Parameters.Add(new SqlParameter("@dataDate", upl.dataDate));
                        cmd.Parameters.Add(new SqlParameter("@FileName", upl.FileName));
                        cmd.Parameters.Add(new SqlParameter("@FileDescription", upl.FileDescription));
                        cmd.Parameters.Add(new SqlParameter("@intMstGroupType", upl.intMstGroupType));
                        cmd.Parameters.Add(new SqlParameter("@intMstDept", upl.intMstDept));
                        cmd.Parameters.Add(new SqlParameter("@Subject", upl.Subject));
                        cmd.Parameters.Add(new SqlParameter("@FileSize", upl.FileSize));
                        cmd.Parameters.Add(new SqlParameter("@Username", username));
                        cmd.Parameters.Add(new SqlParameter("@FileDesc", dt));
                        cmd.Parameters.Add(new SqlParameter("@isOverride", upl.isOverride));

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

        public static List<UploadList> GetUploadList(int catId, int groupType, int Dept, string Username)
        {
            var dbMgr = new dbManager();
            var list = new List<UploadList>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_GetDownloadList";
                        cmd.Parameters.Add(new SqlParameter("@catId", catId));
                        cmd.Parameters.Add(new SqlParameter("@groupType", groupType));
                        cmd.Parameters.Add(new SqlParameter("@Dept", Dept));
                        cmd.Parameters.Add(new SqlParameter("@Username", Username));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new UploadList 
                                {
                                    intTrnUpload = Convert.ToInt32(rdr["intTrnUpload"]),                                    
                                    FileName = rdr["FileName"].ToString(),
                                    FileDescription = rdr["FileDescription"].ToString(),
                                    dataDate = Convert.ToDateTime(rdr["UploadDate"]).ToShortDateString(),
                                    Subject = rdr["Subject"].ToString(),
                                    FileSize = rdr["FileSize"].ToString(),
                                    intMstDept = Convert.ToInt32(rdr["intMstDept"]),
                                    intMstGroupType = Convert.ToInt32(rdr["intMstGroupType"]),
                                    GroupType = rdr["GroupType"].ToString(),
                                    DeptName = rdr["DeptName"].ToString(),
                                    CategoryID = Convert.ToInt32(rdr["CategoryID"])
                                };

                                list.Add(item);
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

        public static Upload GetUpload(int intTrnUpload)
        {
            var dbMgr = new dbManager();
            var item = new Upload();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblTrnUpload WHERE intTrnUpload = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", intTrnUpload));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                item.intTrnUpload = Convert.ToInt32(rdr["intTrnUpload"]);
                                item.CategoryID = Convert.ToInt32(rdr["CategoryID"]);
                                item.FileName = rdr["FileName"].ToString();
                                item.FileDescription = rdr["FileDescription"].ToString();
                                item.intMstDept = Convert.ToInt32(rdr["intMstDept"]);
                                item.intMstGroupType = Convert.ToInt32(rdr["intMstGroupType"]);
                                item.Subject = rdr["Subject"].ToString();
                                item.FileSize = rdr["FileSize"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return item;
        }

        public static string Delete(string fileName, int catID)
        {
            var dbMgr = new dbManager();
            string msg = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Trn_DeleteUpload";
                        cmd.Parameters.Add(new SqlParameter("@FileName", fileName));
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", catID));

                        conn.Open();
                        msg = (string)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return msg;
        }

        public static bool FileIsUseByAnotherCategory(string fileName, int catId) {

            string query = "SELECT intTrnUpload FROM tblTrnUpload WHERE FileName = '" + fileName + "' AND CategoryID <> " + catId;

            return Utilities.isRecordExists(query);
        }
    }
    #endregion

}
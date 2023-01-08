using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    #region DAL
    public static class CategoryDAL
    {
        public static string Save(Category cat)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_AddEditCategory";
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", cat.CategoryID));
                        cmd.Parameters.Add(new SqlParameter("@CategoryName", cat.CategoryName));

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

        public static List<Category> GetCategoryList()
        {
            var dbMgr = new dbManager();
            var items = new List<Category>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM tblMstCategory ORDER BY CategoryName", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new Category 
                                {
                                    CategoryID = Convert.ToInt32(rdr["CategoryID"]),
                                    CategoryName = rdr["CategoryName"].ToString()
                                };

                                items.Add(item);
                            }
                        }
                    }
                }                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return items;

        }

        public static Category GetCategory(int id)
        {
            var dbMgr = new dbManager();
            var item = new Category();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM tblMstCategory WHERE CategoryID = @id", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                
                                item.CategoryID = Convert.ToInt32(rdr["CategoryID"]);
                                item.CategoryName = rdr["CategoryName"].ToString();
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

        public static bool DeleteCategory(int id)
        {
            var dbMgr = new dbManager();
            bool result = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM tblMstCategory WHERE CategoryID = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        conn.Open();
                        int intRes = cmd.ExecuteNonQuery();
                        if (intRes > 0) { result = true; }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message.ToString());
            }

            return result;
        }

        public static List<Category> GetCategoryByUser(int userid, string option)
        { 
            
            var dbMgr = new dbManager();
            var items = new List<Category>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_GetCategoryByUser";
                        cmd.Parameters.Add(new SqlParameter("@UserID", userid));
                        cmd.Parameters.Add(new SqlParameter("@Option", option));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new Category 
                                {
                                    CategoryID = Convert.ToInt32(rdr["CategoryID"]),
                                    CategoryName = rdr["CategoryName"].ToString()
                                };

                                items.Add(item);
                            }
                        }
                    }
                }                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return items;
        }

        public static bool isCategoryUsed(int id) {

            bool result = false;

            if (Utilities.isRecordExists("SELECT CategoryID FROM tblTrnUpload WHERE CategoryID = " + id))
                result = true;

            return result;
        }
    }
    #endregion


}
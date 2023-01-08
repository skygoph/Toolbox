using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class DeptPrefix
    {
        public int intMstPrefix { get; set; }
        public int intMstDepartment { get; set; }
        public string PrefixName { get; set; }
    }

    public class DeptPrefixList
    {
        public int intMstPrefix { get; set; }
        public int intMstDepartment { get; set; }
        public string DepartmentName { get; set; }
        public string PrefixName { get; set; }
    }

    #region DAL
    public static class DeptPrefixDAL
    {
        public static List<DeptPrefixList> GetList()
        {
            var dbMgr = new dbManager();
            var list = new List<DeptPrefixList>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Mst_GetDeptPrefix", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                        

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new DeptPrefixList();

                                item.intMstDepartment = Convert.ToInt32(rdr[0]);
                                item.DepartmentName = rdr[1].ToString();
                                item.PrefixName = rdr[2].ToString();
                                item.intMstPrefix = Convert.ToInt32(rdr[3]);

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

        public static DeptPrefix Get(int intMstDepartment)
        {
            var dbMgr = new dbManager();
            var item = new DeptPrefix();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstDeptPrefix WHERE intMstDepartment = @intMstDepartment";
                        cmd.Parameters.Add(new SqlParameter("@intMstDepartment", intMstDepartment));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                item.intMstPrefix = Convert.ToInt32(rdr[0]);
                                item.intMstDepartment = Convert.ToInt32(rdr[1]);
                                item.PrefixName = rdr[2].ToString();
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

        public static bool Delete(int intMstPrefix)
        {
            var dbMgr = new dbManager();
            bool result = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM tblMstDeptPrefix WHERE intMstPrefix = @intMstPrefix";
                        cmd.Parameters.Add(new SqlParameter("@intMstPrefix", intMstPrefix));

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

        public static string Save(DeptPrefix dp)
        {
            var dbMgr = new dbManager();
            string strResult = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_AddEditDeptPrefix";
                        cmd.Parameters.Add(new SqlParameter("@intMstDeptPrefix", dp.intMstPrefix));
                        cmd.Parameters.Add(new SqlParameter("@intMstDepartment", dp.intMstDepartment));
                        cmd.Parameters.Add(new SqlParameter("@PrefixName", dp.PrefixName));

                        conn.Open();
                        strResult = (string)cmd.ExecuteScalar();
                        if (!strResult.Contains("saved"))
                        {
                            throw new Exception(strResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return strResult;
        }

        public static List<ComboBoxSource> GetDepartment()
        {
            string query = string.Empty;

            query = "SELECT intMstDepartment, DepartmentName FROM tblMstDepartment ORDER BY DepartmentName";

            return OthersDAL.GetDataCombo(query, "ConnectionHRIS");
        }

        public static List<ComboBoxSource> GetDepartmentSP()
        {
            string query = string.Empty;

            query = "[Mst_GetDeptName]";
            return OthersDAL.GetDataComboStored(query);
        }
    }
    #endregion
}
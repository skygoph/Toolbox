using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class Department
    {
        public int intMstDept { get; set; }
        public string DeptName { get; set; }
    }

    #region DAL
    public static class DepartmentDAL
    {
        public static string Save(Department dept)
        {
            var dbMgr = new dbManager();
            string strMsg = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_AddEditDept";
                        cmd.Parameters.Add(new SqlParameter("@intMstDept", dept.intMstDept));
                        cmd.Parameters.Add(new SqlParameter("@DeptName", dept.DeptName));

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

        public static List<Department> GetDeptList()
        {
            var dbMgr = new dbManager();
            var items = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstDepartment ORDER BY DeptName";
                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new Department
                                {
                                    intMstDept = Convert.ToInt32(rdr[0]),
                                    DeptName = rdr[1].ToString()
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

        public static Department GetDept(int id)
        {
            var dbMgr = new dbManager();
            var item = new Department();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstDepartment WHERE intMstDept = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {

                                item.intMstDept = Convert.ToInt32(rdr[0]);
                                item.DeptName = rdr[1].ToString();
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

        public static bool DeleteDept(int id)
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
                        cmd.CommandText = "DELETE FROM tblMstDepartment WHERE intMstDept = @id";
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
    }
    #endregion
}
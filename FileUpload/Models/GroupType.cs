using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class GroupType
    {
        public int intMstGroupType { get; set; }
        public string GroupTypeName { get; set; }
    }

    #region DAL
    public static class GroupTypeDAL
    {
        public static string Save(GroupType gt)
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
                        cmd.CommandText = "Mst_AddEditGroupType";
                        cmd.Parameters.Add(new SqlParameter("@intMstGroupType", gt.intMstGroupType));
                        cmd.Parameters.Add(new SqlParameter("@GroupType", gt.GroupTypeName));

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

        public static List<GroupType> GetGroupTypeList()
        {
            var dbMgr = new dbManager();
            var list = new List<GroupType>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstGroupType ORDER BY GroupType";

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new GroupType();

                                item.intMstGroupType = Convert.ToInt32(rdr[0]);
                                item.GroupTypeName = rdr[1].ToString();

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

        public static GroupType GetGroupType(int intMstGroupType)
        {
            var dbMgr = new dbManager();
            var item = new GroupType();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstGroupType WHERE intMstGroupType = @intMstGroupType";
                        cmd.Parameters.Add(new SqlParameter("@intMstGroupType", intMstGroupType));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {                                
                                item.intMstGroupType = Convert.ToInt32(rdr[0]);
                                item.GroupTypeName = rdr[1].ToString();                                
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

        public static bool DeleteGroupType(int intMstGroupType)
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
                        cmd.CommandText = "DELETE FROM tblMstGroupType WHERE intMstGroupType = @intMstGroupType";
                        cmd.Parameters.Add(new SqlParameter("@intMstGroupType", intMstGroupType));

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

        public static List<GroupType> GetGroupTypeCombo()
        {
            var dbMgr = new dbManager();
            var list = new List<GroupType>();
            string query = "SELECT 0 AS intMstGroupType, '' AS GroupType ";
            query += "UNION ";
            query += "SELECT * FROM tblMstGroupType ORDER BY GroupType";

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new GroupType();

                                item.intMstGroupType = Convert.ToInt32(rdr[0]);
                                item.GroupTypeName = rdr[1].ToString();

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
    }
    #endregion
}
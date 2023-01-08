using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FileUpload.Models
{
    public class ComboBoxSource
    {
        public object ValueMember { get; set; }
        public string DisplayMember { get; set; }
    }


    #region DAL
    public static class OthersDAL
    {
        public static List<ComboBoxSource> GetDataCombo(string strQuery, string dbConnstr = "ConnectionHRIS")
        {
            var dbMgr = new dbManager();
            var source = new List<ComboBoxSource>();

            try
            {
                using (var conn = new SqlConnection(dbMgr.getSQLConnectionString(dbConnstr)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = strQuery;

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var data = new ComboBoxSource()
                                {
                                    ValueMember = rdr[0],
                                    DisplayMember = rdr[1].ToString()
                                };

                                source.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return source;
        }

        public static List<ComboBoxSource> GetDataComboStored(string strQuery)
        {
            var dbMgr = new dbManager();
            var source = new List<ComboBoxSource>();

            try
            {
                using (var conn = new SqlConnection(dbMgr.getSQLConnectionString("Connection")))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = strQuery;

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var data = new ComboBoxSource()
                                {
                                    ValueMember = rdr[0],
                                    DisplayMember = rdr[1].ToString()
                                };

                                source.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return source;
        }
    }
    #endregion
}
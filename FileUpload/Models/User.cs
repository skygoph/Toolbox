using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileUpload.Models
{

    #region User Class

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public int intMstUserGroup { get; set; }        
        public bool isActive { get; set; }
        public bool isAdmin { get; set; }
        public bool hasGroupTaggings { get; set; }
    }

    public class UserOneAccess
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string RightsName { get; set; }
        public int intMstUserGroup { get; set; }
        public bool isActive { get; set; }
        public bool isAdmin { get; set; }    
    }

    public class ChangePassword
    {
        public string Username { get; set; }
        public string CorrectPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UserGroup
    {
        public int intMstUserGroup { get; set; }
        public string UserGroupName { get; set; }

        public virtual ICollection<UserGroupDetailViewData> UserGroupDetail { get; set; }
        public virtual ICollection<ModuleDetail> ModuleDetail { get; set; }
    }    

    public class UserGroupDetail
    {
        public int intMstUserGroupDetail { get; set; }
        public int intMstUserGroup { get; set; }
        public int CategoryID { get; set; }
        public bool canUpload { get; set; }
        public bool canDownload { get; set; }
        public bool canEdit { get; set; }
    }

    public class UserGroupDetailViewData
    {
        public int intMstUserGroupDetail { get; set; }
        public int intMstUserGroup { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool canUpload { get; set; }
        public bool canDownload { get; set; }
        public bool canEdit { get; set; }
    }

    public class Module
    {
        public int intMstModule { get; set; }
        public string ModuleName { get; set; }
    }

    public class ModuleDetail
    {
        public int intMstModuleDetail { get; set; }
        public int intMstModule { get; set; }
        public int intMstUserGroup { get; set; }
        public string ModuleName { get; set; }
        public bool canEdit { get; set; }
        public bool canUpload { get; set; }
        public bool canDownload { get; set; }        
        public int intOrderBy { get; set; }
        public string ControllerName { get; set; }
    }
    #endregion

    #region DAL
    public static class UserDAL
    {
        #region User
        public static User AuthenticateUser(string username, string password)
        {
            var dbMgr = new dbManager();
            var user = new User();

            try
            {
                using (var conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstUser";
                        cmd.CommandTimeout = 180;

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (username == rdr["Username"].ToString() && password == rdr["UserPassword"].ToString())
                                {
                                    user.UserID = Convert.ToInt32(rdr["UserID"]);
                                    user.Username = rdr["Username"].ToString();
                                    user.UserPassword = rdr["UserPassword"].ToString();
                                    user.intMstUserGroup = Convert.ToInt32(rdr["intMstUserGroup"]);
                                    user.isActive = Convert.ToBoolean(rdr["isActive"]);
                                    user.isAdmin = Convert.ToBoolean(rdr["isAdmin"]);
                                    user.hasGroupTaggings = Convert.ToBoolean(rdr["hasGroupTaggings"]);

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return user;
        }

        public static User AuthenticateUser(string username)
        {
            var dbMgr = new dbManager();
            var user = new User();

            try
            {
                using (var conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstUser";
                        cmd.CommandTimeout = 900;

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (username == rdr["Username"].ToString())
                                {
                                    user.UserID = Convert.ToInt32(rdr["UserID"]);
                                    user.Username = rdr["Username"].ToString();
                                    user.UserPassword = rdr["UserPassword"].ToString();
                                    user.intMstUserGroup = Convert.ToInt32(rdr["intMstUserGroup"]);
                                    user.isActive = Convert.ToBoolean(rdr["isActive"]);
                                    user.isAdmin = Convert.ToBoolean(rdr["isAdmin"]);
                                    user.hasGroupTaggings = Convert.ToBoolean(rdr["hasGroupTaggings"]);

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return user;
        }        

        public static List<ViewDataUserList> GetUserList()
        {
            var dbMgr = new dbManager();
            var list = new List<ViewDataUserList>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_GetUser";
                        cmd.Parameters.Add(new SqlParameter("@UserID", 0));
                        cmd.Parameters.Add(new SqlParameter("@whatType", "List"));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new ViewDataUserList
                                {
                                    UserID = Convert.ToInt32(rdr["UserID"]),
                                    Username = rdr["Username"].ToString(),
                                    UserGroupName = rdr["UserGroupName"].ToString(),
                                    isAdmin = Convert.ToBoolean(rdr["isAdmin"])
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

        public static User GetUser(int id)
        {
            var dbMgr = new dbManager();
            var item = new User();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstUser WHERE UserID = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                
                                item.UserID = Convert.ToInt32(rdr["UserID"]);
                                item.Username = rdr["Username"].ToString();
                                item.UserPassword = rdr["UserPassword"].ToString();
                                item.intMstUserGroup = Convert.ToInt32(rdr["intMstUserGroup"]);
                                item.isAdmin = Convert.ToBoolean(rdr["isAdmin"]);
                                item.isActive = Convert.ToBoolean(rdr["isActive"]);
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

        public static UserOneAccess GetUserOneAccess(int id)
        {
            var dbMgr = new dbManager();
            var item = new UserOneAccess();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM tblMstUser WHERE UserID = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {

                                item.UserID = Convert.ToInt32(rdr["UserID"]);
                                item.Username = rdr["Username"].ToString();
                                item.intMstUserGroup = Convert.ToInt32(rdr["intMstUserGroup"]);
                                item.isAdmin = Convert.ToBoolean(rdr["isAdmin"]);
                                item.isActive = Convert.ToBoolean(rdr["isActive"]);
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

        public static bool CheckUserExists(string username)
        {
            string query = "SELECT UserID FROM tblMstUser WHERE Username = '" + username + "'";
            return Utilities.isRecordExists(query);
        }

        public static string SaveUser(UserOneAccess user)
        {
            var dbMgr = new dbManager();
            string message = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_AddEditUser";
                        cmd.Parameters.Add(new SqlParameter("@UserID", user.UserID));
                        cmd.Parameters.Add(new SqlParameter("@Username", user.Username));
                        //cmd.Parameters.Add(new SqlParameter("@UserPassword", user.UserPassword));
                        cmd.Parameters.Add(new SqlParameter("@intMstUserGroup", user.intMstUserGroup));
                        cmd.Parameters.Add(new SqlParameter("@isActive", user.isActive));
                        cmd.Parameters.Add(new SqlParameter("@isAdmin", user.isAdmin));

                        conn.Open();
                        message = (string)cmd.ExecuteScalar();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());                
            }

            return message;

        }

        public static bool DeleteUser(int id)
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
                        cmd.CommandText = "DELETE FROM tblMstUser WHERE UserID = @id";
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

        public static string ChangePassowrd(ChangePassword cp)
        {
            var dbMgr = new dbManager();
            string _result = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_ChangePassword";
                        cmd.Parameters.Add(new SqlParameter("@Username", cp.Username));
                        cmd.Parameters.Add(new SqlParameter("@NewPassword", cp.NewPassword));

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
        #endregion

        #region User Group
        public static UserGroup GetUserGroup(int id)
        {
            var dbMgr = new dbManager();
            var item = new UserGroup();

            try
            {

                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_GetUserGroup";
                        cmd.Parameters.Add(new SqlParameter("@intMstUserGroup", id));
                        cmd.Parameters.Add(new SqlParameter("@whatType", "User Group"));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {                                
                                item.intMstUserGroup = Convert.ToInt32(rdr["intMstUserGroup"]);
                                item.UserGroupName = rdr["UserGroupName"].ToString();                     
                            }

                            item.UserGroupDetail = GetUserGroupDetail(id);
                            item.ModuleDetail = GetModuleDetail(id);
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

        public static List<UserGroup> GetUserGroupList()
        {
            var dbMgr = new dbManager();
            var list = new List<UserGroup>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_GetUserGroup";
                        cmd.Parameters.Add(new SqlParameter("@intMstUserGroup", 0));
                        cmd.Parameters.Add(new SqlParameter("@whatType", "List"));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new UserGroup
                                {                                    
                                    intMstUserGroup = Convert.ToInt32(rdr["intMstUserGroup"]),
                                    UserGroupName = rdr["UserGroupName"].ToString(),
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

        public static List<UserGroupDetailViewData> GetUserGroupDetail(int id)
        {
            var dbMgr = new dbManager();
            var list = new List<UserGroupDetailViewData>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_GetUserGroup";
                        cmd.Parameters.Add(new SqlParameter("@intMstUserGroup", id));
                        cmd.Parameters.Add(new SqlParameter("@whatType", "User Group Detail"));

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new UserGroupDetailViewData 
                                {
                                    intMstUserGroupDetail = Convert.ToInt32(rdr["intMstUserGroupDetail"]),
                                    intMstUserGroup = Convert.ToInt32(rdr["intMstUserGroup"]),
                                    CategoryID = Convert.ToInt32(rdr["CategoryID"]),
                                    CategoryName = rdr["CategoryName"].ToString(),
                                    canUpload = Convert.ToBoolean(rdr["canUpload"]),
                                    canDownload = Convert.ToBoolean(rdr["canDownload"]),
                                    canEdit = Convert.ToBoolean(rdr["canEdit"])
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

        public static string SaveUserGroup(UserGroup group, DataTable dt, DataTable dtM)
        {
            var dbMgr = new dbManager();
            string result = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_AddEditUserGroup";
                        cmd.Parameters.Add(new SqlParameter("@intMstUserGroup", group.intMstUserGroup));
                        cmd.Parameters.Add(new SqlParameter("@UserGroupName", group.UserGroupName));
                        cmd.Parameters.Add(new SqlParameter("@UserGroupDetail", dt));
                        cmd.Parameters.Add(new SqlParameter("@ModuleDetail", dtM));

                        conn.Open();
                        result = (string)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return result;

        }

        public static bool DeleteUserGroup(int id)
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
                        cmd.CommandText = "DELETE FROM tblMstUserGroup WHERE intMstUserGroup = @id";
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
        #endregion

        #region User Module
        public static List<ModuleDetail> GetModuleDetail(int intTrnUserGroup)
        {
            var dbMgr = new dbManager();
            var list = new List<ModuleDetail>();

            try
            {
                using (SqlConnection conn = new SqlConnection(dbMgr.getSQLConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("", conn))
                    {
                        cmd.CommandTimeout = 900;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Mst_GetModules";
                        cmd.Parameters.Add(new SqlParameter("@intMstUserGroup", intTrnUserGroup));                        

                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new ModuleDetail
                                {
                                    intMstModuleDetail = Convert.ToInt32(rdr["intMstModuleDetail"]),
                                    intMstModule = Convert.ToInt32(rdr["intMstModule"]),
                                    intMstUserGroup = Convert.ToInt32(rdr["intMstUserGroup"]),
                                    ModuleName = rdr["ModuleName"].ToString(),
                                    canUpload = Convert.ToBoolean(rdr["canUpload"]),
                                    canDownload = Convert.ToBoolean(rdr["canDownload"]),
                                    canEdit = Convert.ToBoolean(rdr["canEdit"]),
                                    intOrderBy = Convert.ToInt32(rdr["intOrderBy"]),
                                    ControllerName = rdr["ControllerName"].ToString()
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

        #endregion
    }
    #endregion
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Common;

namespace FileUpload.Models
{
    class dbManager
    {
        private string oConnString = string.Empty;

        public string ConnectionName
        {
            get { return oConnString; }
            set { oConnString = value; }
        }

        public string getSQLConnectionString(string connectionName = "Connection")
        {
            ConnectionName = connectionName;

            string strConnection = ConfigurationManager.ConnectionStrings[connectionName].ToString();

            return strConnection;
        }

    }
}

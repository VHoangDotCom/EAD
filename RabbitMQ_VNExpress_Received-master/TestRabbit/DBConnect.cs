using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TestRabbit
{
    class DBConnect
    {
        private static readonly string datasource = @"(localDb)\MSSQLLocalDB";//your server
        private static readonly string database = "RabbitDB"; //your database name
        private static SqlConnection cnn;

        public static SqlConnection connect()
        {
            if (cnn == null)
            {
                //your connection string 
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;";
            cnn = new SqlConnection(connString);
            }
            return cnn;
        }
    }
}

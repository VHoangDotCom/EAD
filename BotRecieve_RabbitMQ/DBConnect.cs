using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BotRecieve_RabbitMQ
{
    class DBConnect
    {
       
        private static SqlConnection cnn;

       /* public static SqlConnection connect()
        {
          *//*  if (cnn == null)
            {
                //your connection string 
                string connString = @"Data Source=" + datasource + ";Initial Catalog="
                            + database + ";Persist Security Info=True;";
                cnn = new SqlConnection(connString);
            }
            return cnn;*//*
        }*/
    }
}

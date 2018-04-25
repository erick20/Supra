using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace Supra.Classes
{
    //public class ApplicationSettings
    //{
    //    public string DBPath { get; set; }
    //}

    public class Datamanager
    {

        //readonly IConfiguration configuration;
        //public Datamanager(IConfiguration config)
        //{
        //    configuration = config;
        //}

        public Datamanager()
        {
            
        }

        private string ConnectionString;
        public string sSQLQueryString = "", sSQLErrorString = "";
        private SqlConnection dbConn = null;
        private SqlCommand cmd = null;


        public static string getValue(string key)
        {
            //return configuration.GetConnectionString(key);
            ResourceManager rm = new ResourceManager("Supra.Resources.Connections", Assembly.GetExecutingAssembly());

            return rm.GetString(key);
              

        }
        

        public SqlConnection getConnection()
        {
            if (dbConn == null || dbConn.State == ConnectionState.Closed)
            {
                try
                {
                    dbConn = new SqlConnection(getValue("SqlConnection"));
                    dbConn.Open();
                }
                catch (Exception ex)
                {
                    Methods.Logger(ex.Message, ex.StackTrace);
                }
            }

            return dbConn;
        }


        public DataTable RunQuery(string sSQL)
        {
            sSQLQueryString = sSQL;
            sSQLErrorString = "";
            SqlConnection conn = getConnection();

            SqlDataAdapter DA = new SqlDataAdapter(sSQL, conn);
            DA.SelectCommand.CommandTimeout = 240;
            DataSet ds = new DataSet();
            DA.Fill(ds);
            DA.Dispose();
            conn.Close();
            return ds.Tables[0];
        }

        public SqlCommand Cmd
        {
            get
            {
                if (cmd == null)
                {
                    cmd = new SqlCommand();
                    cmd.Connection = getConnection();
                }
                return cmd;
            }
            set
            {
                cmd = value;
            }
        }

        public DataRow RunQueryOneRow(string sSQL)
        {
            sSQLQueryString = sSQL;
            sSQLErrorString = "";
            SqlConnection conn = getConnection();

            SqlDataAdapter DA = new SqlDataAdapter(sSQL, conn);
            DA.SelectCommand.CommandTimeout = 240;
            DataSet ds = new DataSet();
            DA.Fill(ds);
            DA.Dispose();

            if (ds.Tables.Count == 0)
            {
                return null;
            }
            else if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            conn.Close();
            return ds.Tables[0].Rows[0];
        }




    }
}

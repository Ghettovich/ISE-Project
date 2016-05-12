using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace AbInitio.Web.App_Start
{
    public class DataConfig : IDbConnection
    {
        private const string dbcon = "Server=localhost; Database=AbInitio; Trusted_Connection=True";
        private SqlConnection sqlConnection = new SqlConnection();
        private SqlCommand sqlCommand = new SqlCommand();
        private SqlDataReader sqlDataReader;
        private SqlDataAdapter sqlAdapter;

        public DataConfig()
        {
            sqlCommand.CommandTimeout = 5;
                        
            sqlConnection.ConnectionString = dbcon;            
        }

        public string ConnectionString
        {
            get
            {
                return dbcon;
            }

            set
            {
                value = dbcon;
            }
        }

        public int ConnectionTimeout
        {
            get
            {
                return sqlConnection.ConnectionTimeout;                
            }
        }

        public string Database
        {
            get
            {
                return sqlConnection.Database;
            }
        }

        public ConnectionState State
        {
            get
            {
                return sqlConnection.State;                
            }
        }

        public IDbTransaction BeginTransaction()
        {
            return sqlConnection.BeginTransaction();
            
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return sqlConnection.BeginTransaction(il);            
        }

        public void ChangeDatabase(string databaseName)
        {
            sqlConnection.ChangeDatabase(databaseName); 
        }

        public void Close()
        {
            sqlConnection.Close();            
        }

        public IDbCommand CreateCommand()
        {
            
            return sqlCommand = sqlConnection.CreateCommand();            
        }

        public IDataAdapter CreateSqlAdapter(SqlCommand cmd)
        {
            sqlAdapter = new SqlDataAdapter(cmd);
            return sqlAdapter;
        }

        public IDataReader CreateSqlReader()
        {
            
            sqlDataReader = sqlCommand.ExecuteReader();
            return sqlDataReader;
        }

        public IDbConnection CreateSqlConnection()
        {
            return sqlConnection;
        }

        public void Dispose()
        {
            sqlConnection.Dispose();            
        }

        public void Open()
        {
            sqlConnection.Open();
            
        }
    }

    public class MyIdentity : IIdentity
    {
        string[] roles = { "0", "1", "2", "3" };
        private GenericPrincipal genericPrincipal;
        private GenericIdentity genericIdentity;
                
        public void CreateIdentity()
        {
            genericIdentity = new GenericIdentity(HttpContext.Current.User.Identity.Name, AuthenticationType);
            genericPrincipal = new GenericPrincipal(genericIdentity, roles);
            HttpContext.Current.User = new GenericPrincipal(genericIdentity, roles);           
        }

        public string AuthenticationType
        {
            get
            {
                
                return "forms";                
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    return true;
                } return false;                
            }
        }

        public string Name
        {
            get
            {
                return genericIdentity.Name;
            }
        }
        
        public bool IsInRole(string role)
        {
            if (genericPrincipal.IsInRole(role))
            {
                return true;
            } return false;
        }
    }

}
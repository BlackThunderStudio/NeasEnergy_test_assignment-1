using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink
{
    public class DBConnect
    {
        private string connString = "data source=.;database=ne-test-database;integrated security=SSPI";
        private SqlConnection conn = null;

        public DBConnect()
        {
            conn = new SqlConnection();
            conn.ConnectionString = connString;
        }

        //In case we want to override the connection from another class
        public SqlConnection GetSqlConnection()
        {
            if (conn != null)
            {
                return conn;
            }
            else throw new DataLayerException("Could not establish a database connection!");
        }

        public SqlDataReader ExecuteSqlStatement(string statement)
        {
            if (statement == null) throw new DataLayerException("Faulty statement", new ArgumentNullException("Statement parameter cannot be null!"));
            if (statement.Equals(String.Empty)) throw new DataLayerException("SQL statement was not found!");

            SqlCommand command = new SqlCommand(statement, conn);            
            SqlDataReader response = null;
            try
            {
                conn.Open();
                response = command.ExecuteReader();
            }
            catch(Exception e){
                throw new DataLayerException("Operation failed! " + e.Message, e);
            }
            finally
            {
                conn.Close();
            }
            return response;
        }
    }
}

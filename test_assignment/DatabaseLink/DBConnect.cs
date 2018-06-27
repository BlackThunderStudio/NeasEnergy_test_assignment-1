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
        private string connString = "Server=tcp:domestosmaximus.database.windows.net,1433;Initial Catalog=ne-assignment-db;Persist Security Info=False;User ID=ne-test;Password=5@h#8yug;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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

        //DEPRECATED!!! left just for comparison
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
                throw new DataLayerException("Operation failed!\n" + e.Message, e);
            }
            finally
            {
                conn.Close();
            }
            return response;
        }
    }
}

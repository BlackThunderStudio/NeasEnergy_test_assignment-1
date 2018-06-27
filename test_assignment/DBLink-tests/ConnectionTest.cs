using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseLink;
using System.Data.SqlClient;

namespace DBLink_tests
{
    [TestClass]
    public class ConnectionTest
    {
        [TestMethod]
        public void ConnectionTest_NotNullConnection()
        {
            DBConnect conn = new DBConnect();
            var result = conn.GetSqlConnection();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DataLayerException))]
        public void ConnectionTest_ExecuteStmtNullParam_expectedDataLayerException()
        {
            DBConnect conn = new DBConnect();
            string param = null;

            conn.ExecuteSqlStatement(param);
        }

        [TestMethod]
        [ExpectedException(typeof(DataLayerException))]
        public void ConnectionTest_ExecuteStmtEmpty_expectedDataLayerException()
        {
            DBConnect conn = new DBConnect();
            string param = "";

            conn.ExecuteSqlStatement(param);
        }

        [TestMethod]
        public void ConnectionTest_ExecuteStmt_success()
        {
            DBConnect conn = new DBConnect();
            string param = "select * from people where Id=1";
            const string EXPECTED = "Mike";

            var link = conn.GetSqlConnection();
            SqlCommand cmd = new SqlCommand(param, link);
            link.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string name = reader.GetString(1);
            link.Close();

            Assert.AreEqual(EXPECTED, name);
        }
    }
}

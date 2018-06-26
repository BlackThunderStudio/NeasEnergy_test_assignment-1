using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseLink;

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
            string param = "select * from salespersons where Id=1";
            const string EXPECTED = "Mike";

            var response = conn.ExecuteSqlStatement(param);
            Assert.IsNotNull(response);
            var name = response.GetString(1);
            Assert.Equals(EXPECTED, name);
        }
    }
}

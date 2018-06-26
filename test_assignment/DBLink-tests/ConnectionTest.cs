using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseLink;

namespace DBLink_tests
{
    [TestClass]
    public class ConnectionTest
    {
        [TestMethod]
        public void NotNullConnection()
        {
            DBConnect conn = new DBConnect();
            var result = conn.GetSqlConnection();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void ExecuteStmtNullParam_expectedDataLayerException()
        {
            DBConnect conn = new DBConnect();
            string param = null;

            conn.ExecuteSqlStatement(param);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void ExecuteStmtEmpty_expectedDataLayerException()
        {
            DBConnect conn = new DBConnect();
            string param = "";

            conn.ExecuteSqlStatement(param);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void ExecuteStmt_success()
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

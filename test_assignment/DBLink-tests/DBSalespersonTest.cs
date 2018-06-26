using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseLink.mapper;
using DatabaseLink.model;
using System.Collections.Generic;
using System.Linq;

namespace DBLink_tests
{
    [TestClass]
    public class DBSalespersonTest
    {
        [TestMethod]
        public void DBSalespersonTest_Get_valid()
        {
            DBSalesperson dBSalesperson = new DBSalesperson();
            const int id = 1;
            const string EXPECTED = "Mike";

            var result = dBSalesperson.Get(id);

            Assert.AreEqual(EXPECTED, result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBSalespersonTest_Get_invalidID()
        {
            DBSalesperson dBSalesperson = new DBSalesperson();
            const int id = -1;

            var result = dBSalesperson.Get(id);

        }

        [TestMethod]
        public void DBSalespersonTest_GetAll()
        {
            DBSalesperson dbconn = new DBSalesperson();

            IEnumerable<Salesperson> result = dbconn.GetAll();

            Assert.AreNotEqual(0, result.ToList().Count);
        }

        [TestMethod]
        public void DBSalespersonTest_Persist()
        {
            DBSalesperson dBSalesperson = new DBSalesperson();
            Salesperson person = new Salesperson() { Name = "Test", Surname = "Object" };

            dBSalesperson.Persist(person);

        }

        [TestMethod]
        public void DBSalespersonTest_Persist_Delete()
        {
            DBSalesperson dBSalesperson = new DBSalesperson();
            Salesperson person = new Salesperson() { Name = "Test", Surname = "Object" };

            dBSalesperson.Persist(person);

            Salesperson salesman = dBSalesperson.GetAll().SingleOrDefault(x =>
            {
                return (x.Name.Equals("Test") && x.Surname.Equals("Object"));
            });

            dBSalesperson.Delete(salesman);

        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBSalespersonTest_Delete_Fail_ID()
        {
            DBSalesperson dBSalesperson = new DBSalesperson();
            Salesperson person = new Salesperson() { Name = "Definitely A", Surname = "Not Existing Person", Id=-1 };

            dBSalesperson.Delete(person);

        }
    }
}

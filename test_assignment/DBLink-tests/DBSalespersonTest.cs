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
            Salesperson person = new Salesperson() { Name = "Test", LastName = "Object" };

            dBSalesperson.Persist(person);

        }

        [TestMethod]
        public void DBSalespersonTest_Persist_Delete()
        {
            DBSalesperson dBSalesperson = new DBSalesperson();
            Salesperson person = new Salesperson() { Name = "Test", LastName = "Object" };

            dBSalesperson.Persist(person);

            Salesperson salesman = dBSalesperson.GetAll().SingleOrDefault(x =>
            {
                return (x.Name.Equals("Test") && x.LastName.Equals("Object"));
            });

            dBSalesperson.Delete(salesman);

        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBSalespersonTest_Delete_Fail_ID()
        {
            DBSalesperson dBSalesperson = new DBSalesperson();
            Salesperson person = new Salesperson() { Name = "Definitely A", LastName = "Not Existing Person", Id=-1 };

            dBSalesperson.Delete(person);

        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBSalespersonTest_Update_Fail_Name()
        {
            DBSalesperson db = new DBSalesperson();
            var person = db.Get(4);
            person.Name = String.Empty;

            db.Update(person);

        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBSalespersonTest_Update_Fail_LastName()
        {
            DBSalesperson db = new DBSalesperson();
            var person = db.Get(3);
            person.LastName = null;

            db.Update(person);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBSalespersonTest_Update_Fail_ID()
        {
            DBSalesperson db = new DBSalesperson();
            var person = db.Get(3);
            person.Id = -2;

            db.Update(person);
        }

        [TestMethod]
        public void DBSalespersonTest_Update()
        {
            DBSalesperson db = new DBSalesperson();
            var person = db.Get(3);
            string nameOld = person.Name;
            string nameNew = "Jason";

            person.Name = nameNew;

            db.Update(person);
            person = null;

            person = db.Get(3);

            Assert.AreEqual(nameNew, person.Name);

            //cleanup
            person.Name = nameOld;
            db.Update(person);
        }
    }
}

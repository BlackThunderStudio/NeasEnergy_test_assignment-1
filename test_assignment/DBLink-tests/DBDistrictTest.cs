using System;
using System.Linq;
using DatabaseLink.mapper;
using DatabaseLink.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DBLink_tests
{
    [TestClass]
    public class DBDistrictTest
    {
        [TestMethod]
        public void DBDistrictTest_AssignSecondary_DeleteSecondary()
        {
            DBDistrict db = new DBDistrict();
            var person = new Salesperson() { Id = 3 };
            var district = new District() { Id = 1 };
            var countOld = db.Get(1).SecondarySalespeople.Count();

            db.AssignSecondary(person, district);

            var countNew = db.Get(1).SecondarySalespeople.Count();
            Assert.AreNotEqual(countOld, countNew);

            db.DeleteSecondary(person, district);

            countNew = db.Get(1).SecondarySalespeople.Count();
            Assert.AreEqual(countOld, countNew);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBDistrictTest_AssignSecondary_fail_PersonID()
        {
            DBDistrict db = new DBDistrict();
            var person = new Salesperson() { Id = -1 };
            var district = new District() { Id = 1 };
            db.AssignSecondary(person, district);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBDistrictTest_AssignSecondary_fail_DistrictID()
        {
            DBDistrict db = new DBDistrict();
            var person = new Salesperson() { Id = 1 };
            var district = new District() { Id = -1 };
            db.AssignSecondary(person, district);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBDistrictTest_DeleteSecondary_fail_PersonID()
        {
            DBDistrict db = new DBDistrict();
            var person = new Salesperson() { Id = -1 };
            var district = new District() { Id = 1 };
            db.DeleteSecondary(person, district);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBDistrictTest_DeleteSecondary_fail_DistrictID()
        {
            DBDistrict db = new DBDistrict();
            var person = new Salesperson() { Id = 1 };
            var district = new District() { Id = -1 };
            db.DeleteSecondary(person, district);
        }

        [TestMethod]
        public void DBDistrictTest_Get()
        {
            DBDistrict db = new DBDistrict();
            var resp = db.Get(1);

            Assert.IsTrue(resp.Name.ToLower().Contains("denmark"));
            Assert.IsNotNull(resp.PrimarySalesperson);
            Assert.IsNotNull(resp.SecondarySalespeople);
            Assert.AreNotEqual(0, resp.SecondarySalespeople.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerException))]
        public void DBDistrictTest_Get_fail_ID()
        {
            DBDistrict db = new DBDistrict();
            db.Get(-1);
        }

        [TestMethod]
        public void DBDistrictTest_GetAll()
        {
            DBDistrict db = new DBDistrict();
            var d = db.GetAll();

            Assert.IsNotNull(d);
            Assert.AreNotEqual(0, d.Count());
        }

        [TestMethod]
        public void DBDistrictTest_Persist_Delete()
        {
            DBDistrict db = new DBDistrict();
            const string dName = "Norway";
            var district = new District() { Name = dName, PrimarySalesperson = new Salesperson() { Id = 2 } };

            var countOld = db.GetAll().Count();
            db.Persist(district);
            var countNew = db.GetAll().Count();

            Assert.AreNotEqual(countOld, countNew);

            district = db.GetAll().ToList().SingleOrDefault(x => x.Name.Equals(dName));

            db.Delete(district);
            countNew = db.GetAll().Count();

            Assert.AreEqual(countOld, countNew);
        }

        [TestMethod]
        public void DBDistrictTest_Update()
        {
            DBDistrict db = new DBDistrict();
            const int id = 2;

            var district = db.Get(id);
            var nameOld = district.Name;
            var nameNew = "Norway";
            district.Name = nameNew;

            db.Update(district);

            district = db.Get(id);
            Assert.AreEqual(nameNew, district.Name);

            district.Name = nameOld;
            db.Update(district);

            district = db.Get(id);
            Assert.AreEqual(nameOld, district.Name);
        }
    }
}

using System;
using DatabaseLink.mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DatabaseLink.model;

namespace DBLink_tests
{
    [TestClass]
    public class DBStoreTest
    {
        [TestMethod]
        public void DBStoreTest_getAll()
        {
            DBStore db = new DBStore();

            var stores = db.GetAll();

            Assert.AreNotEqual(0, stores.ToList().Count);
            Assert.AreEqual("Shop 1", stores.ToList()[0].Name);
            Assert.IsNotNull(stores.ToList()[2].District);
        }

        [TestMethod]  
        [ExpectedException(typeof(DatabaseLink.DataLayerArgumentException))]
        public void DBStoreTest_Get_Fail_ID()
        {
            DBStore db = new DBStore();
            var store = db.Get(-1);
        }

        [TestMethod]
        public void DBStoreTest_Get()
        {
            DBStore db = new DBStore();
            var store = db.Get(2);

            Assert.IsNotNull(store);
            Assert.IsNotNull(store.District);
            Assert.IsNotNull(store.District.PrimarySalesperson);

            Assert.AreEqual("Shop 2", store.Name);
            Assert.AreEqual(2, store.District.Id);
            Assert.AreEqual("Jared", store.District.PrimarySalesperson.Name);
        }

        [TestMethod]
        public void DBStoreTest_Persist_Delete()
        {
            DBStore db = new DBStore();

            var store = new Store() { Name = "Nike", Address = "Milton Road 4420", District = new District() { Id = 2 } };

            db.Persist(store);

            var selected = db.GetAll().ToList().SingleOrDefault(x => { return (x.Name == "Nike" && x.Address == "Milton Road 4420");});

            db.Delete(selected);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerArgumentException))]
        public void DBStoreTest_Delete_Fail_ID()
        {
            DBStore db = new DBStore();

            var store = new Store() { Id = -23, Name = "asdasd", Address = "werwr3" };

            db.Delete(store);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerArgumentException))]
        public void DBStoreTest_Persist_fail_District()
        {
            DBStore db = new DBStore();
            Store store = new Store() { Name = "asdasd", Address = "x" };

            db.Persist(store);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerArgumentException))]
        public void DBStoreTest_Persist_fail_DistrictID()
        {
            DBStore db = new DBStore();
            Store store = new Store() { Name = "asdasd", Address = "asdsadf", District = new District() { Id = -1 } };

            db.Persist(store);
        }

        [TestMethod]
        public void DBStoreTest_Update()
        {
            DBStore db = new DBStore();
            var store = db.Get(2);

            var addressOld = store.Address;
            var addressNew = "New Address 1";
            var districtIdOld = store.District.Id;
            var districtIdNew = 3;

            store.Address = addressNew;
            store.District.Id = districtIdNew;

            db.Update(store);
            store = null;

            store = db.Get(2);
            Assert.AreEqual(addressNew, store.Address);
            Assert.AreEqual(districtIdNew, store.District.Id);

            //cleanup
            store.Address = addressOld;
            store.District.Id = districtIdOld;
            db.Update(store);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerArgumentException))]
        public void DBStoreTest_Update_fail_ID()
        {
            DBStore db = new DBStore();
            Store store = new Store() { Id = -1 };

            db.Update(store);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerArgumentException))]
        public void DBStoreTest_Update_fail_District()
        {
            DBStore db = new DBStore();
            Store store = new Store() { Id = 2 };

            db.Update(store);
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseLink.DataLayerArgumentException))]
        public void DBStoreTest_Update_fail_DistrictID()
        {
            DBStore db = new DBStore();
            Store store = new Store() { Id = 2, District = new District() { Id = -1 } };

            db.Update(store);
        }
    }
}

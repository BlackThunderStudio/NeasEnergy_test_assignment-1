using System;
using System.Linq;
using API.Controllers;
using DatabaseLink.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace API.Tests.Controllers
{
    [TestClass]
    public class StoreControllerTest
    {

        private StoreController controller;

        public StoreControllerTest()
        {
            controller = new StoreController();
        }

        [TestMethod]
        public void StoreControllerTest_GetAll()
        {
            var stores = controller.Get();

            Assert.AreNotEqual(0, stores.ToList().Count);
        }

        [TestMethod]
        public void StoreControllerTest_Get()
        {
            var store = controller.Get(1);
            var expected = "Shop 1";

            Assert.AreEqual(expected, store.Name);
        }

        [TestMethod]
        public void StoreControllerTest_POST_DELETE()
        {
            var store = new Store()
            {
                Id = 20,
                Name = "Shop X",
                Address = "Address X",
                District = new District()
                {
                    Id = 2
                }
            };

            var countOld = controller.Get().Count();
            controller.Post(store);
            var countNew = controller.Get().Count();

            Assert.AreNotEqual(countOld, countNew);

            store = controller.Get().ToList().SingleOrDefault(x => { return (x.Name.Equals(store.Name) && x.Address.Equals(store.Address)); });
            controller.Delete(store);
            countNew = controller.Get().Count();

            Assert.AreEqual(countOld, countNew);
        }

        [TestMethod]
        public void StoreControllerTest_Put()
        {
            var store = controller.Get(1);
            string nameNew = "Shop X";
            string nameOld = store.Name;

            store.Name = nameNew;
            controller.Put(store.Id, store);
            store = controller.Get(store.Id);

            Assert.AreEqual(nameNew, store.Name);
            store.Name = nameOld;
            controller.Put(store.Id, store);
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using ClientApp.ApiController;
using ClientApp.Models;
using ClientApp.Models.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientApp_Tests.ApiController
{
    [TestClass]
    public class StoreControllerTest
    {
        private StoreController controller;

        public StoreControllerTest()
        {
            controller = new StoreController();
            controller.Endpoint = "http://localhost:50209/";
        }

        [TestMethod]
        public void StoreControllerTest_Get()
        {
            Task.Run(async () =>
            {
                var store = await controller.GetAsync(1);
                string expected = "Shop 1";
                Assert.AreEqual(expected, store.Name);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(ApiException))]
        public void StoreControllerTest_Get_fail_ID()
        {
            Task.Run(async () =>
            {
                var result = await controller.GetAsync(999999);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void StoreControllerTest_GetAll()
        {
            Task.Run(async () =>
            {
                var stores = await controller.GetAllAsync();
                Assert.AreNotEqual(0, stores.ToList().Count);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void StoreControllerTest_Post_Delete()
        {
            Task.Run(async () =>
            {
                var store = new Store()
                {
                    Name = "Shop 2137",
                    Address = "Address 2137",
                    District = new District()
                    {
                        Id = 2
                    }
                };
                var p = await controller.GetAllAsync();
                int countOld = p.Count();
                await controller.PersistAsync(store);
                p = await controller.GetAllAsync();
                int countNew = p.Count();

                Assert.AreNotEqual(countOld, countNew);

                int id = p.ToList().SingleOrDefault(x => { return (x.Name.Equals(store.Name) && x.Address.Equals(store.Address)); }).Id;
                await controller.DeleteAsync(id);

                p = await controller.GetAllAsync();
                countNew = p.Count();
                Assert.AreEqual(countOld, countNew);
            }).GetAwaiter().GetResult();
        }
    }
}

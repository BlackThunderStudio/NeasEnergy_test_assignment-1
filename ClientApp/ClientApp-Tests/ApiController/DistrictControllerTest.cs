using System;
using System.Linq;
using System.Threading.Tasks;
using ClientApp.ApiController;
using ClientApp.Models.DatabaseModels;
using ClientApp.Models.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientApp_Tests.ApiController
{
    [TestClass]
    public class DistrictControllerTest
    {

        private DistrictController controller;

        public DistrictControllerTest()
        {
            controller = new DistrictController();
            controller.Endpoint = "http://localhost:50209/";
        }

        [TestMethod]
        public void DistrictControllerTest_GetAll()
        {
            Task.Run(async () =>
            {
                var districts = await controller.GetAllAsync();
                Assert.AreNotEqual(0, districts.ToList().Count);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void DistrictControllerTest_Get()
        {
            Task.Run(async () =>
            {
                var district = await controller.GetAsync(1);
                string expected = "denmark";
                Assert.IsTrue(district.Name.ToLower().Contains(expected));
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(ApiException))]
        public void DistrictControllerTest_Get_fail_ID()
        {
            Task.Run(async () =>
            {
                var result = await controller.GetAsync(999999);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void DistrictControllerTest_Post_Delete()
        {
            Task.Run(async () =>
            {
                var district = new District()
                {
                    Name = "Norway",
                    PrimarySalesperson = new Salesperson()
                    {
                        Id = 5
                    }
                };
                var p = await controller.GetAllAsync();
                int countOld = p.Count();
                await controller.PersistAsync(district);
                p = await controller.GetAllAsync();
                int countNew = p.Count();
                Assert.AreNotEqual(countOld, countNew);
                int id = p.ToList().SingleOrDefault(x => { return (x.Name.Equals(district.Name) && x.PrimarySalesperson.Id.Equals(district.PrimarySalesperson.Id)); }).Id;
                await controller.DeleteAsync(id);
                p = await controller.GetAllAsync();
                countNew = p.Count();
                Assert.AreEqual(countOld, countNew);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void DistrictControllerTest_Update()
        {
            Task.Run(async () =>
            {
                var district = await controller.GetAsync(1);
                var oldName = district.Name;
                var newName = "Norwayy";

                district.Name = newName;
                await controller.UpdateAsync(district);
                district = await controller.GetAsync(district.Id);
                Assert.AreEqual(newName, district.Name);
                district.Name = oldName;
                await controller.UpdateAsync(district);
                district = await controller.GetAsync(district.Id);
                Assert.AreEqual(oldName, district.Name);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void DistrictControllerTest_AssignSecondary_DeleteSecondary()
        {
            Task.Run(async () =>
            {
                var salesman = new Salesperson()
                {
                    Id = 6
                };
                var district = new District()
                {
                    Id = 2
                };

                await controller.AssignSecondaryAsync(salesman, district);

                await controller.DeleteSecondaryAsync(salesman, district);
            }).GetAwaiter().GetResult();
        }
    }
}

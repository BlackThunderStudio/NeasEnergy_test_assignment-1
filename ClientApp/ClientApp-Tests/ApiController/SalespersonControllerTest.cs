using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientApp.ApiController;
using System.Threading.Tasks;
using ClientApp.Models.Exceptions;
using ClientApp.Models;
using System.Linq;

namespace ClientApp_Tests.ApiController
{
    [TestClass]
    public class SalespersonControllerTest
    {
        private SalespersonController controller;

        public SalespersonControllerTest()
        {
            controller = new SalespersonController();
            controller.Endpoint = "http://localhost:50209/";
        }

        [TestMethod]
        public void SalespersonControllerTest_Get()
        {
            Task.Run(async () =>
            {
                var person = await controller.GetAsync(1);
                string expected = "Mike";
                Assert.AreEqual(expected, person.Name);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [ExpectedException(typeof(ApiException))]
        public void SalespersonControllerTest_Get_fail_ID()
        {
            Task.Run(async () =>
            {
                var result = await controller.GetAsync(999999);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void SalespersonControllerTest_GetAll()
        {
            Task.Run(async () =>
            {
                var people = await controller.GetAllAsync();
                Assert.AreNotEqual(0, people.ToList().Count);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void SalespersonControllerTest_Post_Delete()
        {
            Task.Run(async () =>
            {
                var person = new Salesperson()
                {
                    Name = "Kyle",
                    LastName = "Matthews"
                };
                var p = await controller.GetAllAsync();
                int countOld = p.Count();
                await controller.PersistAsync(person);
                p = await controller.GetAllAsync();
                int countNew = p.Count();

                Assert.AreNotEqual(countOld, countNew);

                int id = p.ToList().SingleOrDefault(x => { return (x.Name.Equals(person.Name) && x.LastName.Equals(person.LastName)); }).Id;
                await controller.DeleteAsync(id);

                p = await controller.GetAllAsync();
                countNew = p.Count();
                Assert.AreEqual(countOld, countNew);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void SalespersonControllerTest_Update()
        {
            Task.Run(async () =>
            {
                var person = await controller.GetAsync(4);
                var oldName = person.Name;
                var newName = "Grzegorz";

                person.Name = newName;
                await controller.UpdateAsync(person);
                person = await controller.GetAsync(person.Id);
                Assert.AreEqual(newName, person.Name);

                person.Name = oldName;
                await controller.UpdateAsync(person);
                person = await controller.GetAsync(person.Id);

                Assert.AreEqual(oldName, person.Name);
            }).GetAwaiter().GetResult();
        }
    }
}

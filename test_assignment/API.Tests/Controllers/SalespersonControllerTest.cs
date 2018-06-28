using System;
using API.Controllers;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace API.Tests.Controllers
{
    [TestClass]
    public class SalespersonControllerTest
    {
        [TestMethod]
        public void SalespersonControllerTest_GetAll()
        {
            SalespersonController controller = new SalespersonController();

            var people = controller.Get();

            Assert.AreNotEqual(0, people.ToList().Count);
        }

        [TestMethod]
        public void SalespersonControllerTest_Get()
        {
            SalespersonController controller = new SalespersonController();

            var person = controller.Get(1);

            const string name = "Mike";

            Assert.AreEqual(name, person.Name);
        }
    }
}

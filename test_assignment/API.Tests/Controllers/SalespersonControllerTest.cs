using System;
using API.Controllers;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseLink.model;

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

        [TestMethod]
        public void SalespersonControllerTest_POST_DELETE()
        {
            SalespersonController controller = new SalespersonController();

            var person = new Salesperson()
            {
                Id = 0,
                Name = "Controller",
                LastName = "Test"
            };

            controller.Post(person);

            person = controller.Get().ToList().SingleOrDefault(x => { return (x.Name.Equals(person.Name) && x.LastName.Equals(person.LastName)); });

            controller.Delete(person.Id);
        }

        [TestMethod]
        public void SalespersonControllerTest_PUT()
        {
            SalespersonController controller = new SalespersonController();

            var person = controller.Get(3);

            string nameOld = person.Name;
            string nameNew = "Watson";

            person.Name = nameNew;

            controller.Put(person.Id, person);
            person = null;

            person = controller.Get(3);

            Assert.AreEqual(nameNew, person.Name);

            person.Name = nameOld;
            controller.Put(person.Id, person);
        }
    }
}

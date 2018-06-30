using System;
using System.Linq;
using API.Controllers;
using DatabaseLink.mapper;
using DatabaseLink.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace API.Tests.Controllers
{
    [TestClass]
    public class DistrictControllerTest
    {

        private DistrictController controller;

        public DistrictControllerTest()
        {
            controller = new DistrictController();
        }

        [TestMethod]
        public void DistrictControllerTest_GetAll()
        {
            var d = controller.Get();

            Assert.AreNotEqual(0, d.ToList().Count());
        }

        [TestMethod]
        public void DistrictControllerTest_Get()
        {
            var d = controller.Get(1);
            string expected = "denmark";

            Assert.IsTrue(d.Name.ToLower().Contains(expected));
        }

        [TestMethod]
        public void DistrictControllerTest_POST_DELETE()
        {
            var d = new District()
            {
                Id = 0,
                Name = "Norway",
                PrimarySalesperson = new Salesperson()
                {
                    Id = 4
                }
            };

            var countOld = controller.Get().ToList().Count;
            controller.Post(d);
            var countNew = controller.Get().ToList().Count;

            Assert.AreNotEqual(countOld, countNew);

            d = controller.Get().ToList().SingleOrDefault(x => { return (x.Name.Equals(d.Name) && x.PrimarySalesperson.Id.Equals(d.PrimarySalesperson.Id)); });
            controller.Delete(d.Id);
            countNew = controller.Get().Count();

            Assert.AreEqual(countOld, countNew);
        }

        [TestMethod]
        public void DistrictControllerTest_Put()
        {
            var d = controller.Get(1);
            string nameNew = "Norway";
            string nameOld = d.Name;

            d.Name = nameNew;
            controller.Put(d.Id, d);
            d = controller.Get(d.Id);

            Assert.AreEqual(nameNew, d.Name);

            d.Name = nameOld;
            controller.Put(d.Id, d);
        }

        [TestMethod]
        public void DistrictControllerTest_AssignSecondarySalesperson()
        {
            int person = 2;
            int district = 4;

            controller.AssignSecondaryToDistrict(district, person);
        }

        [TestMethod]
        public void DistrictControllerTest_DeleteSecondarySalesperson()
        {
            int person = 2;
            int district = 4;

            controller.DeleteSecondaryFromDistrict(district, person);
        }
    }
}

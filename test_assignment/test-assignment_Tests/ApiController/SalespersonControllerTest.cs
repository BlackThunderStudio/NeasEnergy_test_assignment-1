using System;
using DatabaseLink.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using test_assignment.ApiController;

namespace test_assignment_Tests.ApiController
{
    [TestClass]
    public class SalespersonControllerTest
    {

        private SalespersonController controller;

        public SalespersonControllerTest()
        {
            controller = new SalespersonController();
        }

        [TestMethod]
        public async void SalespersonControllerTest_GetAsync()
        {
            Salesperson person = await controller.GetAsync(1);

            Assert.IsNotNull(person);
        }
    }
}

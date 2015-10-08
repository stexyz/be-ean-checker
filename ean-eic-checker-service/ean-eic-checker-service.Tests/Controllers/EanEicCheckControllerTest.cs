using Microsoft.VisualStudio.TestTools.UnitTesting;
using ean_eic_checker_service.Controllers;
using ean_eic_checker_service.Models;

namespace ean_eic_checker_service.Tests.Controllers {

    [TestClass]
    public class EanEicCheckControllerTest{

        /// <summary>
        /// All the combinations tested in the EanEicCheckServiceUnitTest; this is just a test for checking the post going through the controller.
        /// </summary>
        [TestMethod]
        public void Post()
        {
            EanEicCheckController controller = new EanEicCheckController();
            Assert.AreEqual(CheckResultCode.EanOk, controller.Post(new EanEicCode("859182400123456789")).ResultCode);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ean_eic_checker_service.Controllers;
using ean_eic_checker_service.Models;

namespace ean_eic_checker_service.Tests.Controllers {

    [TestClass]
    public class EanEicCheckControllerTest{
        [TestMethod]
        public void Post(EanEicCode code)
        {
            EanEicCheckController controller = new EanEicCheckController();
            CheckResult res = controller.Post(new EanEicCode {Code = ""});
            Assert.AreEqual("", res.Description);
        }
    }
}

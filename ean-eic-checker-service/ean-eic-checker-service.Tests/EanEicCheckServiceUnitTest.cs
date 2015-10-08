using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ean_eic_checker_service.Models;
using ean_eic_checker_service.Services;

namespace ean_eic_checker_service.Tests {
    [TestClass]
    public class EanEicCheckServiceUnitTest
    {
        private EanEicCheckService _service;
        private readonly EanEicCode _validEan = new EanEicCode("859182400123456789");
        private readonly EanEicCode _nonDigitEan = new EanEicCode("859x82D00&*(4567▼˘");
        private readonly EanEicCode _invalidSumcheckEan = new EanEicCode("859182400123456784");
        private readonly EanEicCode _tooLongEan = new EanEicCode("859182400123456789654465");
        private readonly EanEicCode _tooShortEan = new EanEicCode("859182400129");


        private readonly EanEicCode _validEic = new EanEicCode("27ZG600Z0424987D");
        private readonly EanEicCode _invalidSumcheckEic = new EanEicCode("27ZG600Z0424987Q");
        private readonly EanEicCode _invalidCharacterEic = new EanEicCode("27ZG6^%$÷˘2▼987D");
        private readonly EanEicCode _tooLongEic = new EanEicCode("27ZG600Z0424987ASDF234D");
        private readonly EanEicCode _tooShortEic = new EanEicCode("27ZG600Z4287D");

        private readonly EanEicCode _invalidPrefix = new EanEicCode("X9ZG600Z0424987D");


        [TestInitialize]
        public void SetUp()
        {
            _service = new EanEicCheckService();

        }

        [TestMethod]
        public void TestValidEan()
        {
            CheckResult res = _service.CheckCode(_validEan);
            Assert.AreEqual("EAN code is ok.", res.Description);
        }

        [TestMethod]
        public void TestValidEic() {
            CheckResult res = _service.CheckCode(_validEic);
            Assert.AreEqual("EIC code is ok.", res.Description);
        }

        [TestMethod]
        public void TestInvalidPrefix() {
            //TODO: validate, that EIC really needs to start with the 27 (in docs example there was EIC starting with '11')
            CheckResult res = _service.CheckCode(_invalidPrefix);
            Assert.AreEqual("Code is not ok, EAN/EIC not recognized (code should start with 85 or 27)", res.Description);
        }

        [TestMethod]
        public void TestTooLongEanLength() {
            CheckResult res = _service.CheckCode(_tooLongEan);
            Assert.AreEqual("EAN code has to have length of 18 characters.", res.Description);
        }

        [TestMethod]
        public void TestTooShortEanLength() {
            CheckResult res = _service.CheckCode(_tooShortEan);
            Assert.AreEqual("EAN code has to have length of 18 characters.", res.Description);
        }

        [TestMethod]
        public void TestTooLongEicLength() {
            CheckResult res = _service.CheckCode(_tooLongEic);
            Assert.AreEqual("EIC code has to have length of 16 characters.", res.Description);
        }

        [TestMethod]
        public void TestTooShortEicLength() {
            CheckResult res = _service.CheckCode(_tooShortEic);
            Assert.AreEqual("EIC code has to have length of 16 characters.", res.Description);
        }

        [TestMethod]
        public void TestEmptyCode() {
            CheckResult res = _service.CheckCode(new EanEicCode(""));
            Assert.AreEqual("No code supplied.", res.Description);
        }

        [TestMethod]
        public void TestNullCode() {
            CheckResult res = _service.CheckCode(new EanEicCode(null));
            Assert.AreEqual("No code supplied.", res.Description);
        }

        [TestMethod]
        public void TestNonDigitCharacterEan() {
            CheckResult res = _service.CheckCode(_nonDigitEan);
            Assert.AreEqual("EAN code is invalid. On the position 4 expecting a digit and got a 'x' character.", res.Description);
        }

        [TestMethod]
        public void TestInvalidCharacterEic() {
            CheckResult res = _service.CheckCode(_invalidCharacterEic);
            Assert.AreEqual("Invalid character in EIC code [" + _invalidCharacterEic.Code + "]. Only 0-9, A-Z and '-' are valid characters.", res.Description);
        }

        [TestMethod]
        public void TestInvalidSumcheckEan() {
            CheckResult res = _service.CheckCode(_invalidSumcheckEan);
            Assert.AreEqual("EAN code is invalid. The checksum is not correct.", res.Description);
        }
    
        [TestMethod]
        public void TestInvalidSumcheckEic() {
            CheckResult res = _service.CheckCode(_invalidSumcheckEic);
            Assert.AreEqual("EIC code is invalid. The checksum is not correct.", res.Description);
        }
    }
}
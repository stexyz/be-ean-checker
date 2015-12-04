using System;
using System.Diagnostics;
using System.IO;
using EndToEndTests.HttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ean_eic_checker_service.Models;
using ean_eic_checker_service.Tests.HttpClient;

namespace EndToEndTests {
    [TestClass]
    public class EanE2ETest {
        private IEanEicCheckerHttpClient httpClient;
        public TestContext TestContext { get; set; }

        private const string SimpleTestPath = "~/EndToEndTests/EAN_Codes.csv";

        [TestInitialize]
        public void SetUp() {
            httpClient = new EanEicCheckerHttpClient(@"http://be-ean-eic-validator.azurewebsites.net/api/EanEicCheck");
        }

        //TODO SP: fix the linking of the csv file
        //[TestMethod]
        public void TestEanCodes() {
            using (StreamReader sr = new StreamReader(SimpleTestPath)) {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null) {
                        Console.WriteLine(currentLine);
                }
            }
//            CheckResult res = httpClient.Post(_validEan);
        }


        private readonly EanEicCode _validEan = new EanEicCode("859182400123456789");
        private readonly EanEicCode _nonDigitEan = new EanEicCode("859x82D00&*(4567▼˘");
        private readonly EanEicCode _invalidSumcheckEan = new EanEicCode("859182400123456784");
        private readonly EanEicCode _tooLongEan = new EanEicCode("859182400123456789654465");
        private readonly EanEicCode _tooShortEan = new EanEicCode("859182400129");
        private readonly EanEicCode _invalidPrefix = new EanEicCode("X9ZG600Z0424987D");


        [TestMethod]
        public void TestEanCodesWithInlineParams()
        {
            CheckResult res = httpClient.Post(_validEan);
            Assert.AreEqual(CheckResultCode.EanOk, res.ResultCode);

            res = httpClient.Post(_invalidPrefix);
            Assert.AreEqual(CheckResultCode.CodePrefixInvalid, res.ResultCode);

            res = httpClient.Post(_tooLongEan);
            Assert.AreEqual(CheckResultCode.EanInvalidLength, res.ResultCode);

            res = httpClient.Post(_tooShortEan);
            Assert.AreEqual(CheckResultCode.EanInvalidLength, res.ResultCode);

            res = httpClient.Post(new EanEicCode(null));
            Assert.AreEqual(CheckResultCode.NoCodeSupplied, res.ResultCode);

            res = httpClient.Post(_nonDigitEan);
            Assert.AreEqual(CheckResultCode.EanInvalidCharacter, res.ResultCode);

            res = httpClient.Post(_invalidSumcheckEan);
            Assert.AreEqual(CheckResultCode.EanInvalidCheckCharacter, res.ResultCode);
        }
    }
}

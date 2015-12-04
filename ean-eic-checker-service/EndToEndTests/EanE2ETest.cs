using System;
using System.Diagnostics;
using System.IO;
using EndToEndTests.HttpClient;
using NUnit.Framework;
using ean_eic_checker_service.Models;
using ean_eic_checker_service.Tests.HttpClient;
using Assert = NUnit.Framework.Assert;

namespace EndToEndTests {
    [TestFixture]
    public class EanE2ETest {
        private IEanEicCheckerHttpClient httpClient;
        public TestContext TestContext { get; set; }

        private const string SimpleTestPath = "EAN_Codes.csv";

        [SetUp]
        public void SetUp() {
            httpClient = new EanEicCheckerHttpClient(@"http://be-ean-eic-validator.azurewebsites.net/api/EanEicCheck");
        }

        [Test]
        public void TestEanCodes() {
            using (StreamReader sr = new StreamReader(SimpleTestPath)) {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    EanEicCode code = new EanEicCode(currentLine.Split(";".ToCharArray())[0]);
                    CheckResultCode expectedResult = (CheckResultCode)Enum.Parse(typeof(CheckResultCode), currentLine.Split(";".ToCharArray())[1]);

                    CheckResult res = httpClient.Post(code);
                    Assert.AreEqual(res.ResultCode, expectedResult);
                }
            }
        }
    }
}

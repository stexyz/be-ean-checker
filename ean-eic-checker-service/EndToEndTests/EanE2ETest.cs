using System;
using System.Diagnostics;
using System.IO;
using EndToEndTests.HttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod]
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
    }
}

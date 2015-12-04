using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using ean_eic_checker_service.Models;
using ean_eic_checker_service.Tests.HttpClient;

namespace EndToEndTests.HttpClient {
    class EanEicCheckerHttpClient : IEanEicCheckerHttpClient {
        private readonly Uri uri;
        private readonly JavaScriptSerializer javaScriptSerializer;

        public EanEicCheckerHttpClient(string endpoint) {
            uri = new Uri(endpoint);
            javaScriptSerializer = new JavaScriptSerializer();
        }

        public CheckResult Post(EanEicCode code) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            var encoding = new UTF8Encoding();

            byte[] bytes = encoding.GetBytes(javaScriptSerializer.Serialize(code));

            request.ContentLength = bytes.Length;

            using (Stream requestStream = request.GetRequestStream()) {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            Stream responseStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            return javaScriptSerializer.Deserialize<CheckResult>(reader.ReadToEnd());
        }
    }
}
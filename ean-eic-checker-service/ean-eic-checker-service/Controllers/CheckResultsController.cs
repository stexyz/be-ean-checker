using System;
using System.Collections.Generic;
using System.Web.Http;
using ean_eic_checker_service.Models;

namespace ean_eic_checker_service.Controllers
{
    public class CheckResultsController : ApiController
    {
        public string[] Get()
        {
            List<string> result = new List<string>();
            foreach (CheckResultCode code in Enum.GetValues(typeof(CheckResultCode)))
            {
                CheckResult cr = new CheckResult(code);
                result.Add(string.Format("Result[{0}], code[{1}], description[{2}]\n", cr.ResultCode.ToString(), (int) cr.ResultCode,
                                         cr.Description));
            }
            return result.ToArray();
        }
    }
}

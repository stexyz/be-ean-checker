using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ean_eic_checker_service.Models;
using ean_eic_checker_service.Services;

namespace ean_eic_checker_service.Controllers
{
    public class EanEicCheckController : ApiController
    {
        private readonly EanEicCheckService _eanEicCheckService;

        public EanEicCheckController()
        {
            _eanEicCheckService = new EanEicCheckService();
        }
        public CheckResult Post(EanEicCode code)
        {
            return _eanEicCheckService.CheckCode(code);
        }
    }
}

using System;
using ean_eic_checker_service.Models;

namespace ean_eic_checker_service.Tests.HttpClient {
    interface IEanEicCheckerHttpClient
    {
        CheckResult Post(EanEicCode code);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//TODO: can I have different subtypes of CheckResult for success and failure?

namespace ean_eic_checker_service.Models {
    public class CheckResult {
        public String Description { get; set; }
    }
}
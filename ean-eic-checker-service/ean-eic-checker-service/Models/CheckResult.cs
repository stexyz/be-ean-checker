using System;

//TODO: can I have different subtypes of CheckResult for success and failure?
//TODO: add result code (cannot assert on description values! :D)
namespace ean_eic_checker_service.Models {
    public class CheckResult {
        public String Description { get; set; }
    }
}
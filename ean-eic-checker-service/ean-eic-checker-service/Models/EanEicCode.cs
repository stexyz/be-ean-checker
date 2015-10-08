using System;

namespace ean_eic_checker_service.Models {
    public class EanEicCode {
        public EanEicCode(string code)
        {
            Code = code;
        }

        public String Code { get; private set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ean_eic_checker_service.Models;

namespace ean_eic_checker_service.Services {
    public class EanEicCheckService {
        public CheckResult CheckCode(EanEicCode code)
        {
            if (code.Code == null)
            {
                return new CheckResult { Description = "No code supplied." };
            }

            //EAN prefix
            String prefix = code.Code.Substring(0, 2);
            if (prefix == "85")
            {
                if (code.Code.Length != 18)
                {
                    return new CheckResult { Description = "EAN code has to have length of 18 characters."};
                }

                // sum = EAN[0] * 3 + EAN[1] + EAN[2] * 3 + EAN[3] + ... + EAN[16] * 3
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    int digit = code.Code[i] - '0';
                    if (digit < 0 || digit > 9)
                    {
                        return new CheckResult{Description ="EAN code is invalid. On the position " + i + " expecting a digit and got a '" + code.Code[i] + "' character."};
                    }
                    if (i % 2 == 0)
                    {
                        sum += digit*3;
                    }
                    else
                    {
                        sum += digit;
                    }
                }

                int lastDigit = code.Code[17] - '0';

                int checkSum = Ceiling(sum, 10) - sum;
                if (lastDigit == checkSum)
                {
                    return new CheckResult {Description = "EAN code is ok."};
                }
                return new CheckResult {Description = "EAN code is invalid. The checksum is not correct."};
            }

            //EIC prefix
            if (prefix == "27")
            {
                if (code.Code.Length != 16) {
                    return new CheckResult { Description = "EIC code has to have length of 16 characters." };
                }
                
                //TODO: finish the check
                return new CheckResult {Description = "EIC recognized. Check to be finished yet."};
            }
            return new CheckResult { Description = "Code is not ok, EAN/EIC not recognized (code should start with 85 or 27)"};
        }

        // Excel formula CEILING(val, sig)
        private static int Ceiling(int value, int significance)
        {
            return value + significance - value % significance;
        }
    }
}
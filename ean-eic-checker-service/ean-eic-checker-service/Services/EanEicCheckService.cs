﻿using System;
using System.Collections.Generic;
using System.Linq;
using ean_eic_checker_service.Models;

namespace ean_eic_checker_service.Services {
    public class EanEicCheckService {
        public CheckResult CheckCode(EanEicCode code)
        {
            if (code == null || string.IsNullOrEmpty(code.Code))
            {
                return new CheckResult(CheckResultCode.NoCodeSupplied);
            }

            //EAN prefix
            if (code.Code.Length >= 2 && code.Code.Substring(0, 2) == "85")
            {
                return ValidateEan(code);
            }

            //EIC prefix
            if (code.Code.Length >= 2 && code.Code.Substring(0, 2) == "27")
            {
                return ValidateEic(code);
            }

            //Prefix did not match
            return new CheckResult(CheckResultCode.CodePrefixInvalid);
        }

        private CheckResult ValidateEic(EanEicCode code)
        {
            if (code.Code.Length != 16)
            {
                return new CheckResult(CheckResultCode.EicInvalidLength);
            }

            // EIC check character computation algorithm from page 32-33 from the https://www.entsoe.eu/Documents/EDI/Library/2015-0612_451-n%20EICCode_Data_exchange_implementation_guide_final.pdf
            IEnumerable<int> numericEncodingOfEic;
            try
            {
                //Step 1&2
                numericEncodingOfEic = GetNumericEicEncoding(code.Code);
            }
            catch (InvalidCharacterInEic)
            {
                return new CheckResult(CheckResultCode.EicInvalidCharacter);
            }

            //Step 3&4
            IEnumerable<int> weightedList = getWeightedListEic(numericEncodingOfEic.Take(15));
            //Step 5
            int sumOfWeights = weightedList.Sum();
            //Step 6
            int modulo37 = 36 - ((sumOfWeights - 1)%37);
            char checkChar = EncodeIntToChar(modulo37);

            if (checkChar == code.Code.Last())
            {
                return new CheckResult(CheckResultCode.EicOk);
            }
            return new CheckResult(CheckResultCode.EicInvalidCheckCharacter);
        }

        private static CheckResult ValidateEan(EanEicCode code)
        {
            if (code.Code.Length != 18)
            {
                return new CheckResult(CheckResultCode.EanInvalidLength);
            }

            // sum = EAN[0] * 3 + EAN[1] + EAN[2] * 3 + EAN[3] + ... + EAN[16] * 3
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                int digit = code.Code[i] - '0';
                if (digit < 0 || digit > 9)
                {
                    return new CheckResult(CheckResultCode.EanInvalidCharacter);
                }
                if (i%2 == 0)
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
                return new CheckResult(CheckResultCode.EanOk);
            }
            return new CheckResult(CheckResultCode.EanInvalidCheckCharacter);
        }

        private static char EncodeIntToChar(int intToEncode)
        {
            if (intToEncode >= 0 && intToEncode <= 9)
            {
                return (char) (intToEncode + '0');
            }

            if (intToEncode >= 10 && intToEncode <= 35)
            {
                return (char) (intToEncode + 'A' - 10);
            }

            if (intToEncode == 36) {
                return '-';
            }

            throw new InvalidCharacterInEic();
        }

        private IEnumerable<int> getWeightedListEic(IEnumerable<int> numericEncodingOfEic)
        {
            List<int> result = new List<int>();
            int i = 16;
            foreach (int val in numericEncodingOfEic)
            {
                result.Add(i-- * val);
            }
            return result;
        }

        private static IEnumerable<int> GetNumericEicEncoding(string code)
        {
            List<int> result = new List<int>();
            foreach (char c in code)
            {
                int encodedChar = EncodeCharToIntEic(c);
                result.Add(encodedChar);
            }
            return result;
        }

        private static int EncodeCharToIntEic(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }

            if (c >= 'A' && c <= 'Z') {
                return c - 'A' + 10;
            }

            if (c == '-') {
                return 36;
            }

            throw new InvalidCharacterInEic();
        }

        /// <summary>
        /// Excel formula CEILING(val, sig)
        /// </summary>
        public static int Ceiling(int value, int significance)
        {
            if (value < 0 || significance < 0)
            {
                throw new ArgumentException("Doesn't support negative parameters.");
            }

            if (significance == 0 || value == 0)
            {
                return 0;
            }

            if (value%significance == 0)
            {
                return value;
            }

            return value + significance - value % significance;
        }

        private class InvalidCharacterInEic : Exception{}
    }
}
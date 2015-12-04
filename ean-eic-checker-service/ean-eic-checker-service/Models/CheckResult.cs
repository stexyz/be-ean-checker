using System;

namespace ean_eic_checker_service.Models {
    public class CheckResult {
        //For serialization only
        public CheckResult(){}

        public CheckResult(CheckResultCode resultCode)
        {
            ResultCode = resultCode;
        }

        public String Description {
            get { return CheckResultDescriptions.ResourceManager.GetString(ResultCode.ToString()); }
        }
        public CheckResultCode ResultCode { get; set; }
    }

    public enum CheckResultCode : uint
    {
        //EAN code is ok.
        EanOk = 0,
        //EIC code is ok.
        EicOk = 1,
        //No code supplied.
        NoCodeSupplied = 2,
        //EAN/EIC not recognized (code should start with 85 or 27)
        CodePrefixInvalid = 3,
        //EAN code has to have the length of 18 characters.
        EanInvalidLength = 4,
        //EIC code has to have the length of 16 characters.
        EicInvalidLength = 5,
        //EAN code is invalid. Only digits are valid characters.
        EanInvalidCharacter = 6,
        //EIC code is invalid. Only 0-9, A-Z and '-' are valid characters.
        EicInvalidCharacter = 7,
        //EAN code is invalid. The check character is incorrect.
        EanInvalidCheckCharacter = 8,
        //EIC code is invalid. The check character  is incorrect.
        EicInvalidCheckCharacter = 9
    }


}
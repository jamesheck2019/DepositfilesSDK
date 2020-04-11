using System;

namespace DepositfilesSDK
{
    public class DepositfilesException : Exception
    {
        public DepositfilesException(string errorMesage, int errorCode) : base(errorMesage) { }
    }
}

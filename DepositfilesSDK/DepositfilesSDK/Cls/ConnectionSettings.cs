using System;

namespace DepositfilesSDK
{
    public class ConnectionSettings
    {
        public TimeSpan? TimeOut = null;
        public bool? CloseConnection = true;
        public ProxyConfig Proxy = null;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class IPAdress
    {
        public static string GetIPv4Adress() {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var ip in host.AddressList) {
                if(!ip.ToString().Contains(':'))
                    return ip.ToString();
            }
            return null;
        }
    }
}

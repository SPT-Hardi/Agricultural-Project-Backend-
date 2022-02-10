using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Middleware
{
    public class UserLoginDetails
    {
        public async Task<string> GetMacAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            String macAddress = string.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (macAddress == string.Empty)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        macAddress = mo["MacAddress"].ToString();
                    }
                }
                mo.Dispose();
            }
            macAddress = macAddress.Replace(":", "-").ToString();
            return macAddress;
        }
    }
}

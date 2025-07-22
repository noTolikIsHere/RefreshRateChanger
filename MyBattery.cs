using System;
using System.Management;
using RefreshRateChanger;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefreshRateChanger
{
    public class MyBattery
    {
        public static ushort GetBatteryChargingStatus()
        {
            ManagementClass c = new ManagementClass("Win32_Battery");
            ManagementObjectCollection c2 = c.GetInstances();
            if (c2.Count == 1)
            {
                using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = c2.GetEnumerator();
                if (managementObjectEnumerator.MoveNext())
                {
                    ManagementObject c3 = (ManagementObject)managementObjectEnumerator.Current;
                    return (ushort)c3.Properties["BatteryStatus"].Value;
                }
            }
            return 0;
        }
    }
}

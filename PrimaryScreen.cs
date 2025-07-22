using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using RefreshRateChanger;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RefreshRateChanger
{
    public class PrimaryScreen
    {
        public struct DEVMODE1
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;

            public short dmSpecVersion;

            public short dmDriverVersion;

            public short dmSize;

            public short dmDriverExtra;

            public int dmFields;

            public short dmOrientation;

            public short dmPaperSize;

            public short dmPaperLength;

            public short dmPaperWidth;

            public short dmScale;

            public short dmCopies;

            public short dmDefaultSource;

            public short dmPrintQuality;

            public short dmColor;

            public short dmDuplex;

            public short dmYResolution;

            public short dmTTOption;

            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;

            public short dmLogPixels;

            public short dmBitsPerPel;

            public int dmPelsWidth;

            public int dmPelsHeight;

            public int dmDisplayFlags;

            public int dmDisplayFrequency;

            public int dmICMMethod;

            public int dmICMIntent;

            public int dmMediaType;

            public int dmDitherType;

            public int dmReserved1;

            public int dmReserved2;

            public int dmPanningWidth;

            public int dmPanningHeight;
        }

        public static uint GetScreenRefreshRate()
        {
            uint result = 0u;
            ManagementClass managementClass = new ManagementClass("Win32_VideoController");
            ManagementObjectCollection instances = managementClass.GetInstances();
            foreach (ManagementObject item in instances)
            {
                if (item.Properties["CurrentRefreshRate"].Value != null)
                {
                    result = (uint)item.Properties["CurrentRefreshRate"].Value;
                }
            }

            return result;
        }

        public static string ChangeRefreshRate(int frequency)
        {
            DEVMODE1 devMode = GetDevMode1();
            if (User_32.EnumDisplaySettings(null, -1, ref devMode) != 0)
            {
                devMode.dmDisplayFrequency = frequency;
                int num = User_32.ChangeDisplaySettings(ref devMode, 2);
                if (num == -1)
                {
                    return "Unable to process your request. Sorry for this inconvenience.";
                }

                return User_32.ChangeDisplaySettings(ref devMode, 1) switch
                {
                    0 => "Success",
                    1 => "You need to reboot for the change to happen.\n If you feel any problems after rebooting your machine\nThen try to change resolution in Safe Mode.",
                    _ => "Failed to change the resolution",
                };
            }

            return "Failed to change the resolution.";
        }

        private static DEVMODE1 GetDevMode1()
        {
            DEVMODE1 dEVMODE = default(DEVMODE1);
            dEVMODE.dmDeviceName = new string(new char[32]);
            dEVMODE.dmFormName = new string(new char[32]);
            dEVMODE.dmSize = (short)Marshal.SizeOf(dEVMODE);
            return dEVMODE;
        }
    }
}

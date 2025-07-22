using RefreshRateChanger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RefreshRateChanger
{
    public class User_32
    {
        public const int ENUM_CURRENT_SETTINGS = -1;

        public const int CDS_UPDATEREGISTRY = 1;

        public const int CDS_TEST = 2;

        public const int DISP_CHANGE_SUCCESSFUL = 0;

        public const int DISP_CHANGE_RESTART = 1;

        public const int DISP_CHANGE_FAILED = -1;

        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string? deviceName, int modeNum, ref RefreshRateChanger.PrimaryScreen.DEVMODE1 devMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref RefreshRateChanger.PrimaryScreen.DEVMODE1 devMode, int flags);
    }
}

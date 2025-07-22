using RefreshRateChanger;
using System.Drawing;
using System.Management;
using System.Windows.Forms;
using Windows.Devices.Power;
using Windows.System;
using Windows.System.Power;

NotifyIcon notifyIcon = new();
notifyIcon.Icon = new Icon(@"\path\to\icon");
notifyIcon.BalloonTipIcon = ToolTipIcon.None;
notifyIcon.BalloonTipTitle = "Screen refresh rate notification";
notifyIcon.BalloonTipClicked += (sender, e) =>
{
    var thisIcon = (NotifyIcon?)sender;
    thisIcon?.Visible = false;
    thisIcon?.Dispose();
};
notifyIcon.BalloonTipClosed += (sender, e) =>
{
    var thisIcon = (NotifyIcon?)sender;
    thisIcon?.Visible = false;
    thisIcon?.Dispose();
};

// Read current brightness
byte currentBrightness = 100;
ManagementScope scope = new ManagementScope("root\\WMI");
SelectQuery brightnessQuery = new SelectQuery("WmiMonitorBrightness");
using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, brightnessQuery))
{
    foreach (ManagementObject obj in searcher.Get())
    {
        currentBrightness = (byte)obj["CurrentBrightness"];
        break;
    }
}

UInt16 b = MyBattery.GetBatteryChargingStatus();
UInt32 currentRefreshRate = PrimaryScreen.GetScreenRefreshRate();

byte pluggedBrightness = 100;
byte unpluggedBrightness = 70;
Int32 pluggedRefreshRate = 165;
Int32 unpluggedRefreshRate = 60;


if (b == 2)
{
    if (currentRefreshRate != pluggedRefreshRate)
    {   
        notifyIcon.Visible = true;
        PrimaryScreen.ChangeRefreshRate(pluggedRefreshRate);
        SetBrightness(pluggedBrightness);
        notifyIcon.BalloonTipText = $"Current screen refresh rate: {pluggedRefreshRate} Hz\nCurrent brightness: {pluggedBrightness} %";
        notifyIcon.ShowBalloonTip(10);
    }
}
else
{
    if (currentRefreshRate != unpluggedRefreshRate)
    {
        notifyIcon.Visible = true;
        PrimaryScreen.ChangeRefreshRate(unpluggedRefreshRate);
        SetBrightness(unpluggedBrightness);
        notifyIcon.BalloonTipText = $"Current screen refresh rate: {unpluggedRefreshRate} Hz\nCurrent brightness: {unpluggedBrightness} %";
        notifyIcon.ShowBalloonTip(10);
    }
}

static void SetBrightness(byte targetBrightness)
{
    ManagementScope scope = new ManagementScope("root\\WMI");
    SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
    {
        foreach (ManagementObject obj in searcher.Get())
        {
            obj.InvokeMethod("WmiSetBrightness", new object[] { UInt32.MaxValue, targetBrightness });
        }
    }
}
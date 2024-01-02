using System.Runtime.InteropServices;
using App1.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using Windows.Devices.Display;
using Windows.Devices.Display.Core;
using Windows.Devices.Enumeration;
using Windows.Graphics;
using Windows.UI.WindowManagement;

namespace App1.Views;

public sealed partial class MainPage : Page
{
    static private List<Window> _activeWindows = new List<Window>();
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var devices = await DeviceInformation.FindAllAsync(DisplayMonitor.GetDeviceSelector());

        foreach (var device in devices)
        {
            var displayMonitor = await DisplayMonitor.FromInterfaceIdAsync(device.Id);

            // Access information about each monitor
            var montiorId = device.Id;
            var monitorScreenWidth = displayMonitor.NativeResolutionInRawPixels.Width;
            var monitorScreenHeight = displayMonitor.NativeResolutionInRawPixels.Height;
            var monitorDpi = displayMonitor.RawDpiX;

            System.Diagnostics.Debug.WriteLine($"montiorId: {montiorId}");
            System.Diagnostics.Debug.WriteLine($"monitorScreenWidth: {monitorScreenWidth}");
            System.Diagnostics.Debug.WriteLine($"monitorScreenHeight: {monitorScreenHeight}");
            System.Diagnostics.Debug.WriteLine($"monitorDpi: {monitorDpi}");
        }
    }

    private async void Button_Click_1(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Window newWindow = new Window
        {
            SystemBackdrop = new MicaBackdrop()
        };

        DisplayMonitor targetDisplay = await GetDisplayByMonitorId("\\\\?\\DISPLAY#DELF141#4&19897eaa&0&UID16843008#{e6f07b5f-ee97-4a90-b076-33f57bf4eaa7}");

        var rootPage = new MainPage();
        newWindow.Content = rootPage;

        newWindow.Activate();


        var monitors = Monitor.All.ToArray();
        if (true)
        {
            var thisMonitor = monitors.LastOrDefault();
            //var otherMonitor = monitors.First(m => m.DeviceName != thisMonitor.DeviceName);
            // move to second display's upper left corner
            newWindow.AppWindow.Move(new PointInt32(thisMonitor.WorkingArea.X, thisMonitor.WorkingArea.Y));
        }

    }


    private async Task<DisplayMonitor> GetDisplayByMonitorId(string monitorId)
    {
        return await DisplayMonitor.FromInterfaceIdAsync(monitorId);
    }
}

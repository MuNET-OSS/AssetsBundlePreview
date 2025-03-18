using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AssetBundlePreview;

public static class Program
{
    [DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    [DllImport("shcore.dll")]
    private static extern int SetProcessDpiAwareness(int value);

    [DllImport("user32.dll")]
    private static extern int SetProcessDpiAwarenessContext(IntPtr dpiContext);

    private static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = new IntPtr(-4);

    
    [STAThread]
    static void Main()
    {
        // 尝试使用最高级别的 DPI Awareness
        if (Environment.OSVersion.Version.Major >= 10)
        {
            SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);
        }
        else if (Environment.OSVersion.Version.Major >= 6) // Windows 8.1 及更早版本
        {
            SetProcessDpiAwareness(2); // 2 = Per Monitor DPI Awareness
        }
        else
        {
            SetProcessDPIAware(); // 仅适用于 Windows 7 及更早版本
        }
        
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);       
        Application.Run(new MainForm());
    }
}
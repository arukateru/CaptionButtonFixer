using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

class Program
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    private static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    private static extern bool DwmIsCompositionEnabled();

    [StructLayout(LayoutKind.Sequential)]
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    static async Task Main()
    {
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;

        if (!DwmIsCompositionEnabled())
        {
            Console.WriteLine("DWM is off.");
            return;
        }

        while (true)
        {

            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                try
                {

                    if (process.ProcessName.Equals("msedge", StringComparison.OrdinalIgnoreCase) ||
                        process.ProcessName.Equals("mblctr", StringComparison.OrdinalIgnoreCase) ||
                        process.ProcessName.Equals("explorer", StringComparison.OrdinalIgnoreCase) ||
                        process.ProcessName.Equals("winver", StringComparison.OrdinalIgnoreCase) ||
                        process.ProcessName.Equals("cmd", StringComparison.OrdinalIgnoreCase) ||
                        process.ProcessName.Equals("chrome", StringComparison.OrdinalIgnoreCase))

                    {
                        continue;
                    }

                    // get handle
                    IntPtr mainWindowHandle = process.MainWindowHandle;


                    MARGINS margins = new MARGINS { cxLeftWidth = 1, cxRightWidth = 1, cyTopHeight = 7, cyBottomHeight = 0 };
                    DwmExtendFrameIntoClientArea(mainWindowHandle, ref margins);

                }
                catch
                {
                    // Ignore processes that do not have a main window handle
                }
            }


            await Task.Delay(800);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ProcessFunctions
    {
        public static void ActivateWindow(string appName)
        {
            string pName = System.IO.Path.GetFileNameWithoutExtension(appName);//要启动的进程名称，可以在任务管理器里查看，一般是不带.exe后缀的;
            Process[] temp = Process.GetProcessesByName(pName);//在所有已启动的进程中查找需要的进程；
            if (temp.Length > 0)//如果查找到
            {
                temp.OrderBy(x => x.StartTime);
                IntPtr handle = temp[0].MainWindowHandle;
                if (handle.ToInt32() == 0)
                {
                    handle = FindMainWindowHandle(temp[0].Id, appName);
                }

                InteropDll.SwitchToThisWindow(handle, true); // 激活，显示在最前
            }
        }

        private static IntPtr FindMainWindowHandle(int processId, string appName)
        {
            IntPtr mainWindowHandle = IntPtr.Zero;
            InteropDll.EnumWindowsProc enumerateHandle = delegate (IntPtr hWnd, int lParam)
            {
                int windowProcessId;
                InteropDll.GetWindowThreadProcessId(hWnd, out windowProcessId);

                if (windowProcessId == processId)
                {
                    var clsName = new StringBuilder(256);
                    InteropDll.GetWindowThreadProcessId(hWnd, out windowProcessId);
                    var hasClass = InteropDll.GetClassName(hWnd, clsName, 256);
                    if (hasClass)
                    {
                        var className = clsName.ToString();

                        if (className.ToLower().Contains(appName.ToLower()) && IsApplicationWindow(hWnd))
                        {
                            mainWindowHandle = hWnd;
                            return false;
                        }
                    }
                }
                return true;
            };
            InteropDll.EnumDesktopWindows(IntPtr.Zero, enumerateHandle, 0);
            return mainWindowHandle;
        }

        private static bool IsApplicationWindow(IntPtr hWnd)
        {
            return (InteropDll.GetWindowLong(hWnd, InteropDll.GWL_EXSTYLE) & InteropDll.WS_EX_APPWINDOW) != 0;
        }
    }
}
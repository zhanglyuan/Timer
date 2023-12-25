/*
 代码出自项目：https://github.com/WPFDevelopersOrg/WPFDevelopers.git
 */

using System.Runtime.InteropServices;

namespace CommonUIBase.Controls.Runtimes.Shell32
{
    public static class Shell32Interop
    {
        [DllImport(Win32.Shell32, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Shell_NotifyIcon([In] NotifyCommand dwMessage, [In] ref NOTIFYICONDATA lpData);

        [DllImport(Win32.Shell32, CharSet = CharSet.Auto)]
        public static extern IntPtr ExtractAssociatedIcon(IntPtr hInst, string iconPath, ref IntPtr index);
    }
}
/*
 代码出自项目：https://github.com/WPFDevelopersOrg/WPFDevelopers.git
 */

using System.Runtime.InteropServices;
using System.Security;

namespace CommonUIBase.Controls.Runtimes
{
    public static class Gdi32Interop
    {
        ///
        /// Critical as suppressing UnmanagedCodeSecurity
        ///
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Win32.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto, EntryPoint = "CreateBitmap")]
        private static extern Interop.BitmapHandle PrivateCreateBitmap(int width, int height, int planes, int bitsPerPixel, byte[] lpvBits);

        ///
        /// Critical - The method invokes PrivateCreateBitmap.
        ///
        [SecurityCritical]
        internal static Interop.BitmapHandle CreateBitmap(int width, int height, int planes, int bitsPerPixel, byte[] lpvBits)
        {
            var hBitmap = PrivateCreateBitmap(width, height, planes, bitsPerPixel, lpvBits);
            var error = Marshal.GetLastWin32Error();

            return hBitmap;
        }

        ///<SecurityNote>
        /// Critical as this code performs an elevation.
        ///</SecurityNote>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Win32.Gdi32, EntryPoint = "DeleteObject", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IntDeleteObject(IntPtr hObject);

        ///<SecurityNote>
        /// Critical: calls a critical method (IntDeleteObject)
        ///</SecurityNote>
        [SecurityCritical]
        public static bool DeleteObject(IntPtr hObject)
        {
            var result = IntDeleteObject(hObject);
            var error = Marshal.GetLastWin32Error();

            return result;
        }

        /// <SecurityNote>
        /// Critical : Elevates to UnmanagedCode permissions
        /// </SecurityNote>
        [SecurityCritical]
        [DllImport(Win32.Gdi32, ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetEnhMetaFileBits(uint cbBuffer, byte[] buffer);

        /// <SecurityNote>
        /// Critical as suppressing UnmanagedCodeSecurity
        /// </SecurityNote>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Win32.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto, EntryPoint = "CreateDIBSection")]
        private static extern Interop.BitmapHandle PrivateCreateDIBSection(HandleRef hdc, ref Interop.BITMAPINFO bitmapInfo, int iUsage, ref IntPtr ppvBits, User32Interop.SafeFileMappingHandle hSection, int dwOffset);

        /// <SecurityNote>
        /// Critical - The method invokes PrivateCreateDIBSection.
        /// </SecurityNote>
        [SecurityCritical]
        internal static Interop.BitmapHandle CreateDIBSection(HandleRef hdc, ref Interop.BITMAPINFO bitmapInfo, int iUsage, ref IntPtr ppvBits, User32Interop.SafeFileMappingHandle hSection, int dwOffset)
        {
            if (hSection == null)
                // PInvoke marshalling does not handle null SafeHandle, we must pass an IntPtr.Zero backed SafeHandle
                hSection = new User32Interop.SafeFileMappingHandle(IntPtr.Zero);

            var hBitmap = PrivateCreateDIBSection(hdc, ref bitmapInfo, iUsage, ref ppvBits, hSection, dwOffset);
            var error = Marshal.GetLastWin32Error();

            return hBitmap;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class ICONINFO
        {
            public bool fIcon = false;
            public Interop.BitmapHandle hbmColor = null;
            public Interop.BitmapHandle hbmMask = null;
            public int xHotspot = 0;
            public int yHotspot = 0;
        }
    }
}
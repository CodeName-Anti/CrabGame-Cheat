using System;
using System.Runtime.InteropServices;

namespace JNNJMods.Utils
{
    /// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-messagebox</see>
    public static class NativeWinAlert
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        public static IntPtr GetWindowHandle()
        {
            return GetActiveWindow();
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int MessageBox(IntPtr hwnd, string lpText, string lpCaption, uint uType);

        /// <summary>
        /// Shows Error alert box with OK button.
        /// </summary>
        /// <param name="text">Main alert text / content.</param>
        /// <param name="caption">Message box title.</param>
        public static void Error(string text, string caption)
        {
            try
            {
                MessageBox(GetWindowHandle(), text, caption, (uint)(0x00000000L | 0x00000010L));
            }
            catch (Exception) { }
        }
    }
}

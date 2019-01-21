using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTitlebarButtons.Native;
using Vanara.Extensions;
using Vanara.PInvoke;

namespace OpenTitlebarButtons.Utils
{
    public partial class NativeThemeUtils
    {
        public enum GWLParameter
        {
            GWL_EXSTYLE = -20, //Sets a new extended window style
            GWL_HINSTANCE = -6, //Sets a new application instance handle.
            GWL_HWNDPARENT = -8, //Set window handle as parent
            GWL_ID = -12, //Sets a new identifier of the window.
            GWL_STYLE = -16, // Set new window style
            GWL_USERDATA = -21, //Sets the user data associated with the window. 
            //This data is intended for use by the application 
            //that created the window. Its value is initially zero.
            GWL_WNDPROC = -4 //Sets a new address for the window procedure.
        }
 
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        internal static extern int SetWindowLong32
            (IntPtr windowHandle, GWLParameter nIndex, int dwNewLong);
 
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64
            (IntPtr windowHandle, GWLParameter nIndex, IntPtr dwNewLong);

        public static int SetWindowLong(IntPtr windowHandle, GWLParameter nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8) //Check if this window is 64bit
            {
                return (int)SetWindowLongPtr64
                    (windowHandle, nIndex, new IntPtr(dwNewLong));
            }
            return SetWindowLong32(windowHandle, nIndex, dwNewLong);
        }

        public static SafeNativeWindowHandle CreateNativeWindow()
        {
            var nWindow = new NativeWindow();
            nWindow.CreateHandle(new CreateParams());
            return nWindow;
        }
    }
}
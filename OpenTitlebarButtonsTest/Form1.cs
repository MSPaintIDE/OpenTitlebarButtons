using System;
using System.Windows.Forms;
using OpenTitlebarButtons.Native;
using OpenTitlebarButtons.Utils;

namespace OpenTitlebarButtonsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            /*var w = new PerPixelAlphaWindow();
            if (NativeThemeUtils.GetDwmWindowButton(AeroTitlebarButtonPart.RestoreButton,
                TitlebarButtonState.Hot) is Bitmap bmp)
            {
                w.SetBitmap(bmp);
                Closing += (s, e) => w.Dispose();
                LocationChanged += (s, e) => RelocateWindow(w);
                SizeChanged += (s, e) => RelocateWindow(w);
                RelocateWindow(w);
                Show();
            }
            else
                w.Dispose();*/
            var frm = new ButtonHoster(null, new NativeUnmanagedWindow(new IntPtr(0x120802)));
            /*frm.LocationChanged += (s, e) =>
            {
                //Console.WriteLine(@"Location changed");
            };*/

        }
    }
}
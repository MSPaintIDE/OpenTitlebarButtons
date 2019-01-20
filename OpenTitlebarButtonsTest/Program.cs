using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenTitlebarButtons;
using OpenTitlebarButtons.Native;
using OpenTitlebarButtons.Utils;

namespace OpenTitlebarButtonsTest
{
    static class Program
    {
        static TaskCompletionSource<object> _showPrm;

    [STAThread]
        static void Main()
        {
            Console.WriteLine(NativeThemeUtils.GetThemesFolder());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            new Thread(() =>
            {
                while (true)
                {
                }
            }).Start();

            Process process = Process.GetProcessesByName("mspaint").FirstOrDefault();
            if (process == null)
            {
                Console.WriteLine("Cannot find any Paint process.");
                return;
            }

            Console.WriteLine("Shit");

            EventManager eventManager = new EventManager();
            
            var frm = new TitlebarButtonHosterForm(eventManager, new NativeUnmanagedWindow(process.MainWindowHandle));
            frm.icon = (Bitmap) Image.FromFile("E:\\InjectTest\\MSPaintIDE\\PaintInjector\\Resources\\run.png");
            frm.XOffset = 173 + 5;
            frm.YOffset = 30;
            
            frm.MouseHover += (sender, args) => { Console.WriteLine("Enter"); };
            frm.MouseLeave += (sender, args) => { Console.WriteLine("Leave"); };
            frm.Click += (sender, args) => { Console.WriteLine("Click"); };

            frm.MouseMove += (sender, args) => { Console.WriteLine("11111111111"); };

        }


        private static Bitmap GetScreenShot(Titlebarinfoex pti)
        {
            var bmpScreenshot = new Bitmap(pti.rcTitleBar.Width,
                pti.rcTitleBar.Height,
                PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            using (var g = Graphics.FromImage(bmpScreenshot))
            {
                g.CopyFromScreen(pti.rcTitleBar.X,
                    pti.rcTitleBar.Y,
                    0,
                    0,
                    pti.rcTitleBar.Size,
                    CopyPixelOperation.SourceCopy);
                var scr = Screen.FromPoint(Cursor.Position);
                using (var bmp2 = new Bitmap(scr.Bounds.Width, scr.Bounds.Height))
                {
                    using (var g2 = Graphics.FromImage(bmp2))
                    {
                        g2.FillRectangle(Brushes.Red,
                            pti.rgrect[(int) CchildrenTitlebarConstants.CchildrenTitlebarMinimizeButton]);
                    }

                    g.DrawImage(bmp2, new Rectangle(0, 0, pti.rcTitleBar.Width, pti.rcTitleBar.Height),
                        new Rectangle(pti.rcTitleBar.X, pti.rcTitleBar.Y, pti.rcTitleBar.Width, pti.rcTitleBar.Height),
                        GraphicsUnit.Pixel);
                }
            }

            return bmpScreenshot;
        }
    }


    ///// <summary>
    ///// The main entry point for the application.
    ///// </summary>
    //[STAThread]
    //static void Main()
    //{
    //    //var atlas = NativeThemeUtils.GetDwmWindowAtlas();
    //    //atlas?.Save("atlas.png", ImageFormat.Png);

    //    NativeThemeUtils.GetDwmWindowButton(TitlebarButtonPart.MinimizeRestoreButtonGlow, TitlebarButtonState.None)?.Save("btnglow.png", ImageFormat.Png);

    //    /*
    //    Application.EnableVisualStyles();
    //    Application.SetCompatibleTextRenderingDefault(false);
    //    Application.Run(new Form1());*/
    //}
}
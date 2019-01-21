using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            
            var frm = new ButtonHoster(eventManager, new NativeUnmanagedWindow(process.MainWindowHandle));
            frm.icon = (Bitmap) Image.FromFile("E:\\InjectTest\\MSPaintIDE\\PaintInjector\\Resources\\run.png");
            frm.XOffset = 173 + 5;
            frm.YOffset = 30;
            
            frm.MouseHover += (sender, args) => { Console.WriteLine("Enter"); };
            frm.MouseLeave += (sender, args) => { Console.WriteLine("Leave"); };
            frm.Click += (sender, args) => { Console.WriteLine("Click"); };

            frm.MouseMove += (sender, args) => { Console.WriteLine("11111111111"); };

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
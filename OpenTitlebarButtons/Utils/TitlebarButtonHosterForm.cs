using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTitlebarButtons.Enums;
using OpenTitlebarButtons.Native;
using System.Resources;
using static Vanara.PInvoke.User32_Gdi;

namespace OpenTitlebarButtons.Utils
{
    public class TitlebarButtonHosterForm : PerPixelAlphaWindow
    {
        private const int WmMouseactivate = 0x0021, MaNoactivate = 0x0003;
        private int _xOffset;
        private int _yOffset;

        public int XOffset
        {
            set
            {
                _xOffset = value;
                Relocate(ParentWindow);
            }
            get => _xOffset;
        }

        public int YOffset
        {
            set
            {
                _yOffset = value;
                Relocate(ParentWindow);
            }
            get => _yOffset;
        }

        private Bitmap _image;

        public Bitmap image
        {
            get => _image;
            set => SetBitmap(_image = value);
        }

        public event EventHandler<CalculateCoordinateEventArgs> CalculateCoords;

        private HandleRef _hwndRef;

        public TitlebarButtonHosterForm(NativeUnmanagedWindow parent)
        {
            AutoScaleMode = AutoScaleMode.None;
            ParentWindow = parent;
            Show(NativeWindow.FromHandle(parent.Handle));
            Attach(parent);
            SetBitmap(NativeThemeUtils.GetDwmWindowButton(AeroTitlebarButtonPart.MinimizeButton,
                TitlebarButtonState.Hot) as Bitmap);
        }

        protected override bool ShowWithoutActivation => true;

        public NativeUnmanagedWindow ParentWindow { get; }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WmMouseactivate)
            {
                m.Result = (IntPtr) MaNoactivate;
                return;
            }

            base.WndProc(ref m);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _hwndRef = new HandleRef(this, Handle);
        }

        private void Attach(NativeUnmanagedWindow parent)
        {
            parent.WindowChanged += (s, e) => Relocate(parent);
            SetWindowPos(new HandleRef(this, Handle), parent.Handle, 0, 0, 0, 0,
                SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE);
            NativeThemeUtils.SetWindowLong(Handle, NativeThemeUtils.GWLParameter.GWL_HWNDPARENT,
                parent.Handle.ToInt32());
            Relocate(parent);
        }

        private void Relocate(NativeUnmanagedWindow parent)
        {
            var loc = parent.Location;

            var args = new CalculateCoordinateEventArgs(loc.X + XOffset, loc.Y + YOffset, parent.Bounds);

            EventHandler<CalculateCoordinateEventArgs> handler = CalculateCoords;
            handler?.Invoke(this, args);

            SetWindowPos(_hwndRef, (IntPtr) 0, args.X, args.Y, 0, 0,
                SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOSIZE);
        }

        public class CalculateCoordinateEventArgs : EventArgs
        {
            public CalculateCoordinateEventArgs(int x, int y, Rectangle bounds)
            {
                X = x;
                Y = y;
                Bounds = bounds;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public Rectangle Bounds { get; }
        }
    }
}
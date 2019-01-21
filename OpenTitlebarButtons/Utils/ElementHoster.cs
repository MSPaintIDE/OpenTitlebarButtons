using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTitlebarButtons.Native;
using static Vanara.PInvoke.User32_Gdi;

namespace OpenTitlebarButtons.Utils
{
    public abstract class ElementHoster : PerPixelAlphaWindow
    {
        private const int WmMouseactivate = 0x0021, MaNoactivate = 0x0003;
        private int _xOffset;
        private int _yOffset;
        public bool hovering;
        public event EventHandler<CalculateCoordinateEventArgs> CalculateCoords;
        public event EventHandler<HoverArgs> Hover;

        public int XOffset
        {
            set
            {
                _xOffset = value;
                Relocate();
            }
            get => _xOffset;
        }

        public int YOffset
        {
            set
            {
                _yOffset = value;
                Relocate();
            }
            get => _yOffset;
        }

        private HandleRef _hwndRef;

        public ElementHoster(EventManager eventManager, NativeUnmanagedWindow parent)
        {
            AutoScaleMode = AutoScaleMode.None;
            ParentWindow = parent;
            Show(NativeWindow.FromHandle(parent.Handle));
            Attach();
            
            eventManager.AddButton(this);
        }

        protected override bool ShowWithoutActivation => true;

        public NativeUnmanagedWindow ParentWindow { get; }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WmMouseactivate)
            {
                OnClick(EventArgs.Empty);
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

        private void Attach()
        {
            ParentWindow.WindowChanged += (s, e) => Relocate();
            SetWindowPos(new HandleRef(this, Handle), ParentWindow.Handle, 0, 0, 0, 0,
                SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE);
            NativeThemeUtils.SetWindowLong(Handle, NativeThemeUtils.GWLParameter.GWL_HWNDPARENT,
                ParentWindow.Handle.ToInt32());
            Relocate();
        }

        public void Relocate()
        {
            var loc = ParentWindow.Location;

            var args = new CalculateCoordinateEventArgs(loc.X + XOffset, loc.Y + YOffset, ParentWindow.Bounds);

            EventHandler<CalculateCoordinateEventArgs> handler = CalculateCoords;
            handler?.Invoke(this, args);

            SetWindowPos(_hwndRef, (IntPtr) 0, args.X, args.Y, 0, 0,
                SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOSIZE);
        }

        internal virtual void OnHover(HoverArgs args)
        {
            EventHandler<HoverArgs> handler = Hover;
            handler?.Invoke(this, args);
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
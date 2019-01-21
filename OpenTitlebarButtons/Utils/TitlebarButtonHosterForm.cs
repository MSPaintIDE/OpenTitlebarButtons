using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTitlebarButtons.Enums;
using OpenTitlebarButtons.Native;
using System.Resources;
using static Vanara.PInvoke.User32_Gdi;
using Gma.System.MouseKeyHook;

namespace OpenTitlebarButtons.Utils
{
    public class TitlebarButtonHosterForm : PerPixelAlphaWindow
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

        // The default icon shown
        private Bitmap _icon;

        public Bitmap icon
        {
            get => _icon;
            set => SetBitmap(_icon = value);
        }

        // When the element is being hovered over
        private Bitmap _hoverIcon;

        public Bitmap hoverIcon
        {
            get => _hoverIcon;
            set => _hoverIcon = value;
        }

        private HandleRef _hwndRef;

        public TitlebarButtonHosterForm(EventManager eventManager, NativeUnmanagedWindow parent)
        {
            AutoScaleMode = AutoScaleMode.None;
            ParentWindow = parent;
            Show(NativeWindow.FromHandle(parent.Handle));
            Attach(parent);
            SetBitmap(NativeThemeUtils.GetDwmWindowButton(AeroTitlebarButtonPart.MinimizeButton,
                TitlebarButtonState.Hot) as Bitmap);
            
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

        internal void OnHover(HoverArgs args)
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
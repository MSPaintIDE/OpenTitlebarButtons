using System.Drawing;
using OpenTitlebarButtons.Native;

namespace OpenTitlebarButtons.Utils
{
    public class ButtonHoster : ElementHoster
    {
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

        public ButtonHoster(EventManager eventManager, NativeUnmanagedWindow parent) : base(eventManager, parent)
        {
            eventManager.AddButton(this);
        }


        internal override void OnHover(HoverArgs args)
        {
            SetBitmap(args.hovering ? hoverIcon ?? icon : icon);
            base.OnHover(args);
        }
    }
}
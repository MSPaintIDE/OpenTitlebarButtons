using System.Drawing;
using System.Windows.Forms;
using OpenTitlebarButtons.Native;

namespace OpenTitlebarButtons.Utils
{
    public class TextHoster : ElementHoster
    {
        public int FontSize { get; set; }
        public Color TextColor { get; set; } = Color.Black;
        public Color BackgroundColor { get; set; } = Color.White;
        private string _text;
        private bool _resizeWidth;
        private bool _resizeHeight;
        private bool _autoFontSize;

        public string DisplayText
        {
            get => _text;
            set => SetText(value);
        }

        public TextHoster(EventManager eventManager, NativeUnmanagedWindow parent) : base(eventManager, parent)
        {
        }

        public void Redraw()
        {
            SetText(_text, _resizeWidth, _resizeHeight, _autoFontSize);
        }

        public void SetText(string text, bool resizeWidth = true, bool resizeHeight = true, bool autoFontSize = false)
        {
            _text = text;
            _resizeWidth = resizeWidth;
            _resizeHeight = resizeHeight;
            _autoFontSize = autoFontSize;

            var width = Width;
            var height = Height;

            var drawFont = new Font(SystemFonts.DefaultFont.Name, autoFontSize ? height : FontSize, GraphicsUnit.Pixel);
            var drawBrush = new SolidBrush(TextColor);

            var size = TextRenderer.MeasureText(string.IsNullOrEmpty(text) ? " " : text, drawFont);
            if (resizeWidth) width = size.Width + 15; // Adding 15 because it seems a bit off sometimes
            if (resizeHeight) height = size.Height == 0 ? Height : size.Height;

            UpdateBounds(Left, Top, width, height);

            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(BackgroundColor);

            graphics.DrawString(text, drawFont, drawBrush, 0, 0, new StringFormat());
            drawBrush.Dispose();

            SetBitmap(bitmap);
        }

        public void ChangeBounds(int x, int y, int width, int height)
        {
            UpdateBounds(x, y, width, height);
        }
    }
}
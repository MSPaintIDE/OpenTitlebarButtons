using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Gma.System.MouseKeyHook;

namespace OpenTitlebarButtons.Utils
{
    public class EventManager
    {
        private Dictionary<ElementHoster, OnHover> buttons =
            new Dictionary<ElementHoster, OnHover>();

        private IKeyboardMouseEvents _globalHook;

        public EventManager()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.MouseMove += (sender, args) =>
            {
                IntPtr hWnd = WindowFromPoint(args.Location);
                
                foreach (var entry in buttons)
                {
                    ElementHoster element = entry.Key;
                    bool nowHovering = element.Handle == hWnd;
                    if (nowHovering != element.hovering)
                    {   
                        element.hovering = nowHovering;
                        entry.Value?.Invoke();
                        
                        var hoverArgs = new HoverArgs();
                        hoverArgs.hovering = nowHovering;
                        element.OnHover(hoverArgs);
                    }
                }
            };
        }

        public IKeyboardMouseEvents GlobalHook => _globalHook;

        public void RemoveButton(ElementHoster button) => buttons.Remove(button);

        public void AddButton(ElementHoster button) => buttons.Add(button, null);
        public void AddButton(ElementHoster button, OnHover onHover) => buttons.Add(button, onHover);

        public delegate void OnHover();
        
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(Point p);
    }
}
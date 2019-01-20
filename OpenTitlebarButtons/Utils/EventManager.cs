using System;
using System.Collections.Generic;
using Gma.System.MouseKeyHook;

namespace OpenTitlebarButtons.Utils
{
    public class EventManager
    {
        private Dictionary<TitlebarButtonHosterForm, OnHover> buttons =
            new Dictionary<TitlebarButtonHosterForm, OnHover>();

        public EventManager()
        {
            IKeyboardMouseEvents globalHook = Hook.GlobalEvents();
            globalHook.MouseMove += (sender, args) =>
            {
                foreach (KeyValuePair<TitlebarButtonHosterForm, OnHover> entry in buttons)
                {
                    TitlebarButtonHosterForm button = entry.Key;
                    if (button.hoverIcon == null) continue;
                    bool nowHovering = button.Bounds.Contains(args.Location);
                    if (nowHovering != button.hovering)
                    {
                        button.SetBitmap(nowHovering ? button.hoverIcon : button.icon);
                        button.hovering = nowHovering;
                        entry.Value?.Invoke();
                    }
                }
            };
        }

        public void AddButton(TitlebarButtonHosterForm button) => buttons.Add(button, null);
        public void AddButton(TitlebarButtonHosterForm button, OnHover onHover) => buttons.Add(button, onHover);

        public delegate void OnHover();
    }
}
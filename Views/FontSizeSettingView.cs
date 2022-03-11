using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Views
{
    class FontSizeSettingView : View
    {
        private SettingEntry<string> settingEntry;
        public static String[] _fontSizes = new string[] { "12", "14", "16", "18", "32" };
        private int textwidth;

        public FontSizeSettingView(SettingEntry<string> se, int width = 100)
        {
            settingEntry = se;
            textwidth = width;
        }

        protected override void Build(Container parentContainer)
        {
            Label fontSize_Label = new Label()
            {
                Location = new Point(5, 4),
                Width = textwidth,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = parentContainer,
                Text = settingEntry.DisplayName,
            };

            Dropdown fontSize_Select = new Dropdown()
            {
                Location = new Point(fontSize_Label.Right + 8, fontSize_Label.Top - 4),
                Width = 60,
                Parent = parentContainer,
            };

            foreach (var s in _fontSizes)
            {
                fontSize_Select.Items.Add(s);
            }
            fontSize_Select.SelectedItem = settingEntry.Value;
            fontSize_Select.ValueChanged += delegate {
                settingEntry.Value = fontSize_Select.SelectedItem;
            };

            parentContainer.Height = fontSize_Select.Height + 10;
        }
    }
}

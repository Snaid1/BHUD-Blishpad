using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Views
{
    class SettingRedirectView : View
    {
        private int buttonWidth = StandardButton.DEFAULT_CONTROL_WIDTH;
        private int buttonheight = StandardButton.STANDARD_CONTROL_HEIGHT;

        protected override void Build(Container buildPanel)
        {
            StandardButton redirectButton = new StandardButton()
            {
                Text = "Open Settings",
                Parent = buildPanel,
                Left = (buildPanel.Width / 2) - (buttonWidth / 2),
                Top = (buildPanel.Height / 2) - (buttonheight / 2)
            };
            redirectButton.Click += delegate
            {
                if (NotesModule._notesManager != null)
                {
                    NotesModule._notesManager.OpenSettings();
                }
            };
        }
    }
}

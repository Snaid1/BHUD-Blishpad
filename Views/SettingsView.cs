using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Blish_HUD.Settings.UI.Views;
using Blish_HUD.Graphics.UI;

namespace Snaid1.BlishHudNotepad.Views
{
    class SettingsView : View
    {
        protected override void Build(Container buildPanel)
        {
            Panel parentPanel = new Panel()
            {
                CanScroll = false,
                Parent = buildPanel,
                Height = buildPanel.Height,
                Width = buildPanel.Width
            };

            NotesModule.getPostIt().getPostItSettingPanel(parentPanel);
        }
    }
}

using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Snaid1.Blishpad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Views
{

    class NotesWindowSettingView : View
    {

        public NotesWindowSettingView()
        {
        }

        protected override void Build(Container buildPanel)
        {
            IView settingShouldShowNotesView_View = SettingView.FromType(NotesModule._settingShouldShowNotesTab , buildPanel.Width);
            ViewContainer settingShouldShowNotesView_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, 10),
                Parent = buildPanel
            };
            settingShouldShowNotesView_Container.Show(settingShouldShowNotesView_View);

            IView settingNotesManagerFontSize_View = new FontSizeSettingView(NotesModule._settingNotesManagerFontSize, 160);
            ViewContainer settingNotesManagerFontSize_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingShouldShowNotesView_Container.Bottom + 10),
                Parent = buildPanel
            };
            settingNotesManagerFontSize_Container.Show(settingNotesManagerFontSize_View);
        }
        public Panel BuildNotesManagerSettingsPanel()
        {
            Panel NotesSettingPanel = new Panel()
            {
                CanScroll = false,
                HeightSizingMode = SizingMode.AutoSize,
                Title = "Notes Manager Settings"
            };

            Build(NotesSettingPanel);
            return NotesSettingPanel;
        }

        public Panel BuildNotesManagerSettingsPanel(Container parentContainer)
        {
            Panel notesPanel = BuildNotesManagerSettingsPanel();
            notesPanel.Parent = parentContainer;
            notesPanel.Width = parentContainer.Width;
            return notesPanel;
        }
    }
}

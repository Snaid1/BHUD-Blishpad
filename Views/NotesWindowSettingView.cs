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
        private NotesWindow notesWindow;
        private NotesManager notesManager;

        public NotesWindowSettingView(NotesWindow notesWindow)
        {
            this.notesWindow = notesWindow;
            this.notesManager = notesWindow.getNotesManager();
        }

        protected override void Build(Container buildPanel)
        {
            IView settingShouldShowNotesView_View = SettingView.FromType(notesManager._settingShouldShowNotesView , buildPanel.Width);
            ViewContainer settingShouldShowNotesView_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, 10),
                Parent = buildPanel
            };
            settingShouldShowNotesView_Container.Show(settingShouldShowNotesView_View);
        }
        public Panel buildNotesManagerSettingsPanel()
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
    }
}

using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Views
{
    class GeneralSettingView : View
    {
        protected override void Build(Container buildPanel)
        {
            IView settingShouldShowNotesView_View = SettingView.FromType(NotesModule._settingIconTogglesPostIt, buildPanel.Width);
            ViewContainer settingShouldShowNotesView_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, 10),
                Parent = buildPanel
            };
            settingShouldShowNotesView_Container.Show(settingShouldShowNotesView_View);

            IView settingShowNotificationOnCopy_View = SettingView.FromType(NotesModule._settingShowNotificationOnCopy, buildPanel.Width);
            ViewContainer settingShowNotificationOnCopy_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingShouldShowNotesView_Container.Bottom + 10),
                Parent = buildPanel
            };
            settingShowNotificationOnCopy_Container.Show(settingShowNotificationOnCopy_View);

        }
        public Panel BuildGeneralSettingsPanel()
        {
            Panel generalPanel = new Panel()
            {
                CanScroll = false,
                HeightSizingMode = SizingMode.AutoSize,
                Title = "General Settings"
            };

            Build(generalPanel);
            return generalPanel;
        }
        public Panel BuildGeneralSettingsPanel(Container ParentPanel)
        {
            Panel generalPanel = BuildGeneralSettingsPanel();

            generalPanel.Parent = ParentPanel;
            generalPanel.Width = ParentPanel.Width;

            return generalPanel;
        }
    }
}

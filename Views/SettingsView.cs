using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Blish_HUD.Settings.UI.Views;
using Blish_HUD.Graphics.UI;

namespace Snaid1.Blishpad.Views
{
    class SettingsView : View
    {
        private Container buildContainer;

        public SettingsView()
        {
        }
        protected override void Build(Container buildPanel)
        {
            buildContainer = buildPanel;

            Panel parentPanel = new FlowPanel()
            {
                CanScroll = false,
                Parent = buildPanel,
                Height = buildPanel.Height,
                Width = buildPanel.Width - 20,
                FlowDirection = ControlFlowDirection.SingleTopToBottom
            };

            var postItPanel = BuildPostItSettingsPanel(parentPanel);

            var notesPanel = BuildNotesManagerSettingsPanel(parentPanel);

            var generalPanel = BuildGeneralSettingsPanel(parentPanel);
        }

        protected Panel BuildPostItSettingsPanel(Container parentPanel)
        {
            PostItSettingView postItSettings = new PostItSettingView();
            Panel postItPanel = postItSettings.BuildPostItSettingsPanel(parentPanel);
            if (buildContainer is Panel)
            {
                postItPanel.ShowBorder = false;
                postItPanel.Title = "";
            }
            return postItPanel;
        }

        protected Panel BuildNotesManagerSettingsPanel(Container parentPanel)
        {
            NotesWindowSettingView notesWindowSettings = new NotesWindowSettingView();
            Panel notesWindowPanel = notesWindowSettings.BuildNotesManagerSettingsPanel(parentPanel);
            if(buildContainer is Panel)
            {
                notesWindowPanel.ShowBorder = false;
                notesWindowPanel.Title = "";
            }
            return notesWindowPanel;
        }

        protected Panel BuildGeneralSettingsPanel(Container parentPanel)
        {
            GeneralSettingView generalSettings = new GeneralSettingView();
            Panel generalPanel = generalSettings.BuildGeneralSettingsPanel(parentPanel);
            if(buildContainer is Panel)
            {
                generalPanel.ShowBorder = false;
                generalPanel.Title = "";
            }
            return generalPanel;
        }
    }
}

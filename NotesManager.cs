using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Snaid1.Blishpad.Controls;
using Snaid1.Blishpad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad
{
    class NotesManager
    {
        //settings
        internal SettingEntry<bool> _settingShouldShowNotesView;

        private NotesModule module;
        private NotesWindow notesWindow;
        private Panel _notesSettingView;

        public NotesManager(NotesModule notesModule)
        {
            module = notesModule;
            notesWindow = new NotesWindow(module, this);
        }

        public TabbedWindow2 getWindow()
        {
            return notesWindow.getWindow();
        }

        internal void DefineSettings(SettingCollection settings)
        {
            _settingShouldShowNotesView = settings.DefineSetting("ShouldShowNotesView", true, () => "Enable Notes Manager Page", () => "Enables the notes tab in the Notes Manager window");

            _settingShouldShowNotesView.SettingChanged += ChangeShouldShowNotesView;
        }

        private void ChangeShouldShowNotesView(object sender, ValueChangedEventArgs<bool> e)
        {
            notesWindow?.reloadTabs();
        }

        internal void Initialize()
        {
            notesWindow.Initialize();
        }

        internal void LoadAsync()
        {
            
        }

        internal void OnModuleLoaded()
        {
            notesWindow.OnModuleLoaded();
        }

        internal void Unload()
        {

        }

        public Panel getSettingsPanel(Container parnetPanel)
        {
            if (_notesSettingView == null) { _notesSettingView = new NotesWindowSettingView(this.notesWindow).buildNotesManagerSettingsPanel(); }
            _notesSettingView.Parent = parnetPanel;
            _notesSettingView.Width = parnetPanel.Width;
            return _notesSettingView;
        }
    }
}

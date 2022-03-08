using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Snaid1.Blishpad.Controls;
using Snaid1.Blishpad.Views;
using Snaid1.Blishpad.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;

namespace Snaid1.Blishpad.Controller
{
    class NotesWindowController
    {
        //settings

        private NotesWindow notesWindow;
        

        public NotesWindowController(ContentsManager contentsManager)
        {
            notesWindow = new NotesWindow(contentsManager, this);
        }

        public TabbedWindow2 getWindow()
        {
            return notesWindow.getWindow();
        }

        public void ShowWindow()
        {
            notesWindow.show();
        }

        public void ToggleWindow()
        {
            notesWindow.ToggleWindow();
        }

        internal void DefineSettings(SettingCollection settings)
        {
            NotesModule._settingShouldShowNotesTab = settings.DefineSetting("ShouldShowNotesView", true, () => "Enable Notes Manager Page", () => "Enables the notes tab in the Notes Manager window");

            NotesModule._settingShouldShowNotesTab.SettingChanged += ChangeShouldShowNotesView;
        }

        private void ChangeShouldShowNotesView(object sender, ValueChangedEventArgs<bool> e)
        {
            notesWindow?.reloadTabs();
        }

        internal void Initialize()
        {
            notesWindow.Initialize();
        }

        public async Task LoadAsync()
        {
            
        }

        internal void OnModuleLoaded()
        {
            notesWindow.OnModuleLoaded();
        }

        internal void Unload()
        {
            NotesModule._settingShouldShowNotesTab.SettingChanged -= ChangeShouldShowNotesView;
        }
    }
}

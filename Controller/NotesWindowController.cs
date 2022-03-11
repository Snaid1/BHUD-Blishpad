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
using Snaid1.Blishpad.Utility;

namespace Snaid1.Blishpad.Controller
{
    public class NotesWindowController
    {
        //settings

        private NotesWindow notesWindow;
        public ContentsManager contentsManager;

        public NotesWindowController(ContentsManager contentsManager)
        {
            this.contentsManager = contentsManager;
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

        public void OpenSettings()
        {
            notesWindow.OpenSettings();
        }
        internal void DefineSettings(SettingCollection settings)
        {
            NotesModule._settingShouldShowNotesTab = settings.DefineSetting("ShouldShowNotesView", true, () => "Enable Notes Manager Page", () => "Enables the notes tab in the Notes Manager window");
            NotesModule._settingNotesManagerFontSize = settings.DefineSetting("NotesManagerFontSize", "16", () => "Notes Manager Font Size", () => "The font size for the Notes Manager tab");

            NotesModule._settingShouldShowNotesTab.SettingChanged += ChangeShouldShowNotesView;
            NotesModule._settingNotesManagerFontSize.SettingChanged += delegate
            {
                if(notesWindow?.notesTabContents?.NoteContentsBox?.Font != null)
                {
                    notesWindow.notesTabContents.NoteContentsBox.Font = BlishpadUtility.getFont(NotesModule._settingNotesManagerFontSize.Value);
                }
            };

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

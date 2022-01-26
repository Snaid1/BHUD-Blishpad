using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Snaid1.Blishpad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Controls
{
    class NotesWindow
    {
        private TabbedWindow2 _window;

        private NotesModule module;
        private NotesManager manager;

        private Tab notesTab;
        private Tab settingsTab;

        public NotesWindow(NotesModule notesModule, NotesManager notesManager)
        {
            module = notesModule;
            manager = notesManager;
        }

        public TabbedWindow2 getWindow()
        {
            return _window;
        }

        private TabbedWindow2 BuildWindow()
        {
            if(_window == null)
            {
                Rectangle windowRec = new Rectangle(-10, 20, 800, 650);
                Rectangle contentRec = new Rectangle(windowRec.X + 40, windowRec.Y + 5, windowRec.Width - 10, windowRec.Height - 10);
                _window = new TabbedWindow2(module.ContentsManager.GetTexture(@"textures\155985.png"), windowRec , contentRec)
                {

                    Parent = Blish_HUD.GameService.Graphics.SpriteScreen,
                    Title = "Blishpad",
                    Emblem = module.ContentsManager.GetTexture(@"textures\parchment.png"),
                    Subtitle = "Notes",
                    SavesPosition = true,
                    Id = $"{nameof(NotesModule)}_NotesModule_995A840DE116AB0805655673E1C4930851149861252974B00F9DE4ACEF762578"
                };
            }

            reloadTabs();

            return _window;
        }

        internal void Initialize()
        {
            notesTab = new Tab(module.ContentsManager.GetTexture(@"textures/icon.png"), () => new NotesWindowView(this), "Notes");
            settingsTab = new Tab(Blish_HUD.GameService.Content.GetTexture("155052"), () => new SettingsView(module), "Settings");
            BuildWindow();


        }

        internal void Unload()
        {
            
        }

        internal void OnModuleLoaded()
        {
            _window.Show();
        }

        public void reloadTabs()
        {
            _window.Tabs.Clear();
            if (manager._settingShouldShowNotesView.Value)
            {
                _window.Tabs.Add(notesTab);
            }
            _window.Tabs.Add(settingsTab);
        }

        public Panel getNotesWindowSettingView()
        {
            return new NotesWindowSettingView(this).buildNotesManagerSettingsPanel();
        }

        public NotesManager getNotesManager()
        {
            return manager;
        }
    }
}

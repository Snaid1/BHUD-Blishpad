using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Snaid1.Blishpad.Controller;
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
        private NotesWindowController controller;

        public ContentsManager contentsManager;

        public Tab notesTab;
        public NotesTabView notesTabContents;
        private Tab settingsTab;

        public NotesWindow(ContentsManager contentsManager, NotesWindowController notesWindowController)
        {
            this.contentsManager = contentsManager;
            controller = notesWindowController;
        }
        public TabbedWindow2 getWindow()
        {
            return _window;
        }

        public void ToggleWindow()
        {
            if(_window != null)
            {
                _window.ToggleWindow();
            }
        }
        public void show()
        {
            if(_window != null)
            {
                _window.Show();
            }
        }

        private TabbedWindow2 BuildWindow()
        {
            if(_window == null)
            {
                Rectangle windowRec = new Rectangle(-20, 20, 925, 710);
                Rectangle contentRec = new Rectangle(windowRec.X + 47, windowRec.Y + 5, windowRec.Width - 5, windowRec.Height - 10);
                _window = new TabbedWindow2(contentsManager.GetTexture(@"textures\155985.png"), windowRec , contentRec)
                {

                    Parent = Blish_HUD.GameService.Graphics.SpriteScreen,
                    Title = "Blishpad",
                    Emblem = contentsManager.GetTexture(@"textures\parchment.png"),
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

        }

        internal void Unload()
        {
            
        }

        internal void OnModuleLoaded()
        {
            notesTabContents = new NotesTabView(this);
            notesTab = new Tab(contentsManager.GetTexture(@"textures/icon.png"), () => notesTabContents, "Notes Manager");
            settingsTab = new Tab(Blish_HUD.GameService.Content.GetTexture("155052"), () => new SettingsView(), "Settings");
            BuildWindow();
        }

        public void reloadTabs()
        {
            _window.Tabs.Clear();
            if (NotesModule._settingShouldShowNotesTab.Value)
            {
                _window.Tabs.Add(notesTab);
            }
            _window.Tabs.Add(settingsTab);
        }

        public NotesWindowController GetNotesWindowController()
        {
            return controller;
        }
    }
}

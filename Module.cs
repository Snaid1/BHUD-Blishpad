using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Overlay.UI.Views;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Snaid1.BlishHudNotepad.Controls;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Snaid1.BlishHudNotepad
{
    [Export(typeof(Blish_HUD.Modules.Module))]
    public class NotesModule : Blish_HUD.Modules.Module
    {

        private static readonly Logger Logger = Logger.GetLogger(typeof(Module));

        internal static NotesModule ModuleInstance;

        #region Service Managers
        internal SettingsManager SettingsManager => this.ModuleParameters.SettingsManager;
        internal ContentsManager ContentsManager => this.ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => this.ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager => this.ModuleParameters.Gw2ApiManager;
        #endregion

        [ImportingConstructor]
        public NotesModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { ModuleInstance = this; }

        //Settings
        internal SettingEntry<bool> _settingShowPostItWindow;
        internal SettingEntry<float> _settingPostItOpacity;
        internal SettingEntry<float> _settingPostItOpacityFocused;
        internal SettingEntry<bool> _settingPostItAlwaysOnTop;


        private TabbedWindow2 _notesWindow;
        private StandardWindow _postItWindow;
        private CornerIcon _notesIcon;
        protected override void DefineSettings(SettingCollection settings)
        {
            _settingShowPostItWindow = settings.DefineSetting("ShowPostItWindow", true, "Enable Post-It Note", "");
            _settingPostItOpacity = settings.DefineSetting("PostItOpacity", 0.8f, "Post-It Opacity (unfocused)", "");
            _settingPostItOpacityFocused = settings.DefineSetting("PostItOpacityFocused", 1.0f, "Post-It Opacity (focused)", "");
            _settingPostItAlwaysOnTop = settings.DefineSetting("PostItAlwaysOnTop", false, "Post-It Always On Top", "");

            _settingShowPostItWindow.SettingChanged += UpdatePostItSettings;
            _settingPostItOpacity.SettingChanged += UpdatePostItSettings;
            _settingPostItOpacityFocused.SettingChanged += UpdatePostItSettings;
            _settingPostItAlwaysOnTop.SettingChanged += UpdatePostItSettings;

        }

        protected override void Initialize()
        {
            _notesWindow = new TabbedWindow2(Blish_HUD.GameService.Content.GetTexture("controls/window/502049"), new Rectangle(0, 0, 545, 630), new Rectangle(82, 30, 467, 600))
            {
                Parent = Blish_HUD.GameService.Graphics.SpriteScreen,
                Title = "TabbedWindow",
                Emblem = Blish_HUD.GameService.Content.GetTexture("controls/window/156022"),
                Subtitle = "Example Subtitle",
                SavesPosition = true,
                //Id = $"{nameof(ExampleClass)}_ExampleModule_38d37290-b5f9-447d-97ea-45b0b50e5f56"
            };

            _notesWindow.Tabs.Add(new Tab(Blish_HUD.GameService.Content.GetTexture("155052"), () => new OverlaySettingsView(), "Settings"));
            _notesWindow.Tabs.Add(new Tab(Blish_HUD.GameService.Content.GetTexture("155052"), () => new AboutView(), "About Blish HUD"));

            //_notesWindow.Show();

            
            createPostIt();
            

            _notesIcon = new CornerIcon()
            {
                IconName = "Blishpad",
                Icon = ContentsManager.GetTexture(@"textures\icon.png"),
                HoverIcon = ContentsManager.GetTexture(@"textures\icon-active.png"),
                Priority = 5
            };

            _notesIcon.Click += delegate { _postItWindow.ToggleWindow(); };
        }

        protected override async Task LoadAsync()
        {
        }

        protected override void OnModuleLoaded(EventArgs e)
        {

            // Base handler must be called
            base.OnModuleLoaded(e);
        }

        protected override void Update(GameTime gameTime)
        {

        }

        /// <inheritdoc />
        protected override void Unload()
        {
            // Unload
            _notesIcon?.Dispose();

            _notesWindow?.Dispose();
            _postItWindow?.Dispose();


            _settingShowPostItWindow.SettingChanged -= UpdatePostItSettings;
            _settingPostItOpacity.SettingChanged -= UpdatePostItSettings;
            _settingPostItOpacityFocused.SettingChanged -= UpdatePostItSettings;


            // All static members must be manually unset
            ModuleInstance = null;
        }

        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<float> e = null)
        {
            createPostIt();
        }

        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
        {
            if (_settingShowPostItWindow.Value)
            {
                createPostIt();
            } else
            {
                _postItWindow?.Dispose();
            }
        }

        private void createPostIt()
        {
            _postItWindow?.Dispose();

            _postItWindow = new StandardWindow(ContentsManager.GetTexture(@"textures\1909316.png"), new Rectangle(0, 20, 445, 555), new Rectangle(35, 19, 467, 600))
            {
                Parent = Blish_HUD.GameService.Graphics.SpriteScreen,
                SavesPosition = true,
                Title = "Post-It Notes",
                Id = $"{nameof(NotesModule)}_NotesModulePostIt_38d38590-b5f9-447d-97ea-37b0b50e5e56",
                TopMost = _settingPostItAlwaysOnTop.Value,
                Opacity = _settingPostItOpacity.Value,
                CanResize = true
            };
            var postItTextBox = new MultilineTextBox()
            {
                Parent = _postItWindow,
                PlaceholderText = "Enter notes here ...",
                Size = new Point(358, 500),
                Font = GameService.Content.DefaultFont16,
                Location = new Point(0, 0)
            };

            _postItWindow.MouseEntered += delegate { _postItWindow.Opacity = _settingPostItOpacityFocused.Value/100; };
            _postItWindow.MouseLeft += delegate { _postItWindow.Opacity = _settingPostItOpacity.Value/100; };

            _postItWindow.Show();
        }

    }

}

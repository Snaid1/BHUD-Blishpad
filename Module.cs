using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Overlay.UI.Views;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snaid1.Blishpad.Controller;
using Snaid1.Blishpad.Controls;
using Snaid1.Blishpad.Utility;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Snaid1.Blishpad
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
        public NotesModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { ModuleInstance = this; notesManager = new NotesWindowController(ContentsManager); }


        private static PostItWindow PostIt = new PostItWindow();
        public PostItWindow postItWindow { get{ return PostIt; } }
        internal NotesWindowController notesManager;
        //Settings
        public static SettingEntry<bool> _settingIconTogglesPostIt;

        // Post-It Settings
        public static SettingEntry<bool> _settingShowPostItWindow;
        public static SettingEntry<float> _settingPostItOpacity;
        public static SettingEntry<float> _settingPostItOpacityFocused;
        public static SettingEntry<bool> _settingPostItAlwaysOnTop;
        public static SettingEntry<PostItWindow.PostItSize> _settingPostItSize;
        public static SettingEntry<string> _settingPostItFontSize;
        public static SettingEntry<bool> _settingEscClosesPostIt;
        public static SettingEntry<bool> _settingPreservePostItContents;
        public static SettingEntry<KeyBinding> _settingPostItToggleKey;

        // Notes Manager Settings
        public static SettingEntry<bool> _settingShouldShowNotesTab;
        public static SettingEntry<string> _settingNotesWindowFontSize;

        private CornerIcon _notesIcon;

        protected override void DefineSettings(SettingCollection settings)
        {
            PostIt.DefineSettings(settings);
            notesManager.DefineSettings(settings);

            _settingIconTogglesPostIt = settings.DefineSetting("IconTogglesPostIt", false, () => "Icon Toggles Post-It", () => "If enabled, causes the icon to toggle the Post-It instead of opening the Blishpad Window.");
        }

        protected override void Initialize()
        {
            FileHelper.NotesDirectory = DirectoriesManager.GetFullDirectoryPath("notes");
            PostIt.ContentsManager = ContentsManager;
            PostIt.Initialize();

            notesManager.Initialize();
        }

        protected override async Task LoadAsync()
        {
            await PostIt.LoadAsync();
            await notesManager.LoadAsync();
        }

        protected override void OnModuleLoaded(EventArgs e)
        {
            PostIt.OnModuleLoaded();
            notesManager.OnModuleLoaded();

            _notesIcon = CreateBlishpadIcon();

            _notesIcon.Click += delegate {
                if (_settingIconTogglesPostIt.Value)
                {
                    PostIt.ToggleWindow();
                } else
                {
                    notesManager.ToggleWindow();
                }
            };

            // Base handler must be called
            base.OnModuleLoaded(e);
        }

        private CornerIcon CreateBlishpadIcon()
        {
           return new CornerIcon()
            {
                IconName = "Blishpad",
                Icon = ContentsManager.GetTexture(@"textures\icon.png"),
                HoverIcon = ContentsManager.GetTexture(@"textures\icon-active.png"),
                Priority = 5
            };
        }
        protected override void Update(GameTime gameTime)
        {

        }

        /// <inheritdoc />
        protected override void Unload()
        {
            // Unload
            _notesIcon?.Dispose();

            PostIt?.Unload();
            notesManager?.Unload();

            // All static members must be manually unset
            ModuleInstance = null;
        }

        public static PostItWindow getPostIt()
        {
            return PostIt;
        }

        public override IView GetSettingsView()
        {
            Blishpad.Views.SettingsView settingview = new Blishpad.Views.SettingsView();
            return settingview;
        }


    }

}

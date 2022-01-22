using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Overlay.UI.Views;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        internal FileHelper FileManager;

        [ImportingConstructor]
        public NotesModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { ModuleInstance = this; }


        private static PostItWindow PostIt = new PostItWindow();
        //Settings



        //private TabbedWindow2 _notesWindow;
        private CornerIcon _notesIcon;

        protected override void DefineSettings(SettingCollection settings)
        {
            PostIt?.DefineSettings(settings);
        }

        protected override void Initialize()
        {
            FileManager = new FileHelper(DirectoriesManager);
            PostIt.SetManagers(SettingsManager, ContentsManager, DirectoriesManager, Gw2ApiManager, FileManager);
            PostIt.Initialize();

            /*_notesWindow = new TabbedWindow2(Blish_HUD.GameService.Content.GetTexture("controls/window/502049"), new Rectangle(0, 0, 545, 630), new Rectangle(82, 30, 467, 600))
            {
                Parent = Blish_HUD.GameService.Graphics.SpriteScreen,
                Title = "TabbedWindow",
                Emblem = Blish_HUD.GameService.Content.GetTexture("controls/window/156022"),
                Subtitle = "Example Subtitle",
                SavesPosition = true,
                //Id = $"{nameof(ExampleClass)}_ExampleModule_38d37290-b5f9-447d-97ea-45b0b50e5f56"
            };

            _notesWindow.Tabs.Add(new Tab(Blish_HUD.GameService.Content.GetTexture("155052"), () => new OverlaySettingsView(), "Settings"));
            _notesWindow.Tabs.Add(new Tab(Blish_HUD.GameService.Content.GetTexture("155052"), () => new AboutView(), "About Blish HUD"));*/

            //_notesWindow.Show();

            
            
            

            _notesIcon = new CornerIcon()
            {
                IconName = "Blishpad",
                Icon = ContentsManager.GetTexture(@"textures\icon.png"),
                HoverIcon = ContentsManager.GetTexture(@"textures\icon-active.png"),
                Priority = 5
            };

            _notesIcon.Click += delegate { PostIt.ToggleWindow(); };
        }

        protected override async Task LoadAsync()
        {
            PostIt.LoadAsync();
        }

        protected override void OnModuleLoaded(EventArgs e)
        {
            PostIt.OnModuleLoaded();
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

            //_notesWindow?.Dispose();
            PostIt?.Unload();




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

using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Overlay.UI.Views;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public NotesModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { ModuleInstance = this; notesManager = new NotesManager(this); }


        private static PostItWindow PostIt = new PostItWindow();
        internal NotesManager notesManager;
        //Settings



        private TabbedWindow2 _notesWindow;
        private CornerIcon _notesIcon;

        protected override void DefineSettings(SettingCollection settings)
        {
            PostIt.DefineSettings(settings);
            notesManager.DefineSettings(settings);
        }

        protected override void Initialize()
        {
            FileHelper.NotesDirectory = DirectoriesManager.GetFullDirectoryPath("notes");
            PostIt.SetManagers(SettingsManager, ContentsManager, DirectoriesManager, Gw2ApiManager);
            PostIt.Initialize();

            notesManager.Initialize();

            _notesWindow = notesManager.getWindow();

            //_notesWindow.Tabs.Add(new Tab(Blish_HUD.GameService.Content.GetTexture("155052"), () => new Blishpad.Views.SettingsView(), "Settings"));
            //_notesWindow.Tabs.Add(new Tab(Blish_HUD.GameService.Content.GetTexture("155052"), () => new AboutView(), "About Blish HUD"));*/

            //_notesWindow.Show();

            
            
            

            _notesIcon = new CornerIcon()
            {
                IconName = "Blishpad",
                Icon = ContentsManager.GetTexture(@"textures\icon.png"),
                HoverIcon = ContentsManager.GetTexture(@"textures\icon-active.png"),
                Priority = 5
            };

            _notesIcon.Click += delegate { _notesWindow.ToggleWindow(); };
        }

        protected override async Task LoadAsync()
        {
            PostIt.LoadAsync();
            notesManager.LoadAsync();
        }

        protected override void OnModuleLoaded(EventArgs e)
        {
            PostIt.OnModuleLoaded();
            notesManager.OnModuleLoaded();
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
            Blishpad.Views.SettingsView settingview = new Blishpad.Views.SettingsView(this);
            return settingview;
        }

        public Panel GetNotesManagerSettingView(Container parnetPanel)
        {
            return notesManager.getSettingsPanel(parnetPanel);
        }

    }

}

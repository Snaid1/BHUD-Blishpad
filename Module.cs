using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Overlay.UI.Views;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;
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
        internal FileHelper FileManager;

        [ImportingConstructor]
        public NotesModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { ModuleInstance = this; }

        internal enum PostItSize
        {
            XSmall,
            Small,
            Medium,
            Large,
            XLarge
        }

        //Settings
        internal SettingEntry<bool> _settingShowPostItWindow;
        internal SettingEntry<float> _settingPostItOpacity;
        internal SettingEntry<float> _settingPostItOpacityFocused;
        internal SettingEntry<bool> _settingPostItAlwaysOnTop;
        internal SettingEntry<PostItSize> _settingPostItSize;
        internal SettingEntry<String> _settingPostItContents;


        private TabbedWindow2 _notesWindow;
        private StandardWindow _postItWindow;
        private CornerIcon _notesIcon;

        private string PostItText;
        private string PostItFile;
        protected override void DefineSettings(SettingCollection settings)
        {
            
            _settingShowPostItWindow = settings.DefineSetting("ShowPostItWindow", true, () => "Enable Post-It Note", () => "Enables a notes window that can be kept On Screen");
            _settingPostItSize = settings.DefineSetting("PostItWindowize", PostItSize.Medium, () => "Post-It Note Size", () => "Sets the size of the Post-It window");
            _settingPostItOpacity = settings.DefineSetting("PostItOpacity", 80.0f, () => "Post-It Opacity (unfocused)", () => "Percentage of how Opaque/Transparent the Post It Notes window should be when not focused");
            _settingPostItOpacityFocused = settings.DefineSetting("PostItOpacityFocused", 100.0f, () => "Post-It Opacity (focused)", () => "Percentage of how Opaque/Transparent the Post It Notes window should be when focused");
            _settingPostItAlwaysOnTop = settings.DefineSetting("PostItAlwaysOnTop", false, () => "Post-It Always On Top", () => "When Checked will cause the Post-It window to be on top of any other windows");
            var PostItHiddenSettings = settings.AddSubCollection("HiddenPostItSettings", false);
            _settingPostItContents = PostItHiddenSettings.DefineSetting("PostItContents", "Write your notes here!", () => null, () => null);

            _settingShowPostItWindow.SettingChanged += UpdatePostItSettings;
            _settingPostItOpacity.SettingChanged += UpdatePostItSettings;
            _settingPostItOpacityFocused.SettingChanged += UpdatePostItSettings;
            _settingPostItAlwaysOnTop.SettingChanged += UpdatePostItSettings;
            _settingPostItSize.SettingChanged += UpdatePostItSettings;

        }

        protected override void Initialize()
        {
            FileManager = new FileHelper(DirectoriesManager);
            PostItFile = "_PostIt";
            PostItText = FileManager.ReadFile(PostItFile);

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

            
            CreatePostIt();
            

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

        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<PostItSize> e = null)
        {
            CreatePostIt();
        }
        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<float> e = null)
        {
            assignPostItOpacity(false,false);
        }

        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
        {
            if (_settingShowPostItWindow.Value)
            {
                CreatePostIt();
            } else
            {
                _postItWindow?.Dispose();
            }
        }

        private void CreatePostIt()
        {
            _postItWindow?.Dispose();
            bool mouseOn = false;
            float scale = 1f;
            Texture2D pageTexture;

            switch (_settingPostItSize.Value)
            {
                case PostItSize.XSmall:
                    pageTexture = ContentsManager.GetTexture(@"textures\1909316-XS.png");
                    scale = 0.5f;
                    break;
                case PostItSize.Small:
                    pageTexture = ContentsManager.GetTexture(@"textures\1909316-S.png");
                    scale = 0.75f;
                    break;
                case PostItSize.Large:
                    pageTexture = ContentsManager.GetTexture(@"textures\1909316-L.png");
                    scale = 1.25f;
                    break;
                case PostItSize.XLarge:
                    pageTexture = ContentsManager.GetTexture(@"textures\1909316-XL.png");
                    scale = 1.5f;
                    break;
                default:
                    pageTexture = ContentsManager.GetTexture(@"textures\1909316-M.png");
                    scale = 1f;
                    break;
            }

            Rectangle windowRec = new Rectangle(0, 20, 418, 535);
            Rectangle contentRec = new Rectangle(30, 18, 398, 515);

            windowRec = ScaleRectangle(windowRec, scale);
            contentRec = ScaleRectangle(contentRec, scale);

            _postItWindow = new StandardWindow(pageTexture, windowRec, contentRec)
            {
                Parent = Blish_HUD.GameService.Graphics.SpriteScreen,
                SavesPosition = true,
                Title = "Post-It",
                Id = $"{nameof(NotesModule)}_NotesModulePostIt_38d38590-b5f9-447d-97ea-37b0b50e5e56",
                TopMost = _settingPostItAlwaysOnTop.Value,
                Opacity = _settingPostItOpacity.Value / 100,
                CanResize = false,
                CanClose = false,
                HeightSizingMode = SizingMode.Standard,
                WidthSizingMode = SizingMode.Standard
            };
            var postItTextBox = new MultilineTextBox()
            {
                Parent = _postItWindow,
                PlaceholderText = "Enter notes here ...",
                Size = ScalePoint(new Point(358, 500), scale),
                Font = GameService.Content.DefaultFont16,
                Location = new Point(0, 0),
                Text = PostItText

            };

            _postItWindow.MouseEntered += delegate { mouseOn = true; assignPostItOpacity(mouseOn, postItTextBox.Focused); };
            _postItWindow.MouseLeft += delegate {
                mouseOn = false;
                assignPostItOpacity(mouseOn, postItTextBox.Focused);
            };
            //_postItWindow.Resized += delegate { updatePostitSize(); };

            postItTextBox.InputFocusChanged += delegate
            {
                assignPostItOpacity(mouseOn, postItTextBox.Focused);
                if(postItTextBox.Focused == false )
                {
                    FileManager.WriteFile(PostItFile, postItTextBox.Text);
                }
            };

            
            _postItWindow.Show();
            assignPostItOpacity(mouseOn, postItTextBox.Focused);
        }

        private void assignPostItOpacity(Boolean mouseOn, Boolean focused)
        {
            var opacityFocused = _settingPostItOpacityFocused.Value / 100;
            var opacityUnfocused = _settingPostItOpacity.Value / 100;

            if (focused || mouseOn)
            {
                _postItWindow.Opacity = opacityFocused;
            } else
            {
                _postItWindow.Opacity = opacityUnfocused;
            }
        }

        /*private void updatePostitSize()
        {
            Rectangle windowbounds = _postItWindow.AbsoluteBounds;
            ResizeTexture(ContentsManager.GetTexture(@"textures\1909316.png"), windowbounds);
        }

        /*private Texture2D ResizeTexture(Texture2D oldTexture, Rectangle newbounds)
        {
            return ResizeTexture(oldTexture, newbounds.Width, newbounds.Height);
        }
        private Texture2D ResizeTexture(Texture2D oldTexture, int newwidth, int newheight)
        {
            var newTexture = oldTexture;
            var newRectangle = new Rectangle(0, 0, newwidth, newheight);

            SpriteBatch batch = new SpriteBatch(Blish_HUD.GameService.Graphics.GraphicsDevice);
            if(_postItWindow != null)
            {
                batch.Begin();
                batch?.DrawOnCtrl(_postItWindow, oldTexture, _postItWindow.ContentRegion, oldTexture.Bounds, Color.White);
                batch.End();
                


            }

            return newTexture;
        }*/

        private int ScaleInt(int num, float scale)
        {
            var newNum = Convert.ToInt32(num * scale);
            return newNum;
        }
        private Point ScalePoint(Point point, float scale)
        {
            var newPoint = new Point(ScaleInt(point.X, scale), ScaleInt(point.Y, scale));
            return newPoint;
        }

        private Rectangle ScaleRectangle(Rectangle rect, float scale)
        {
            var newRect = new Rectangle(ScaleInt(rect.X, scale), ScaleInt(rect.Y, scale), ScaleInt(rect.Width, scale), ScaleInt(rect.Height, scale));
            return newRect;
        }

    }

}

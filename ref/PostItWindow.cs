using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Overlay.UI.Views;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.BlishHudNotepad
{
    public class PostItWindow
    {
        internal SettingsManager SettingsManager;
        internal ContentsManager ContentsManager;
        internal DirectoriesManager DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager;
        internal FileHelper FileManager;

        internal enum PostItSize
        {
            XSmall,
            Small,
            Medium,
            Large,
            XLarge
        }
        internal static String[] _fontSizes = new string[] { "12", "14", "16", "18", "32" };
        //Settings
        internal SettingEntry<bool> _settingShowPostItWindow;
        internal SettingEntry<float> _settingPostItOpacity;
        internal SettingEntry<float> _settingPostItOpacityFocused;
        internal SettingEntry<bool> _settingPostItAlwaysOnTop;
        internal SettingEntry<PostItSize> _settingPostItSize;
        internal SettingEntry<string> _settingPostItFontSize;

        private StandardWindow _postItWindow { get; set; }

        private Panel _postItSettingsPanel;


        private string PostItText;
        private string PostItFile;
        public void SetManagers(SettingsManager sm, ContentsManager cm, DirectoriesManager dm, Gw2ApiManager apim, FileHelper fm)
        {
            SettingsManager = sm;
            ContentsManager = cm;
            DirectoriesManager = dm;
            Gw2ApiManager = apim;
            FileManager = fm;
        }
        public void DefineSettings(SettingCollection settings)
        {
            _settingShowPostItWindow = settings.DefineSetting("ShowPostItWindow", true, () => "Enable Post-It Note", () => "Enables a notes window that can be kept On Screen");
            _settingPostItSize = settings.DefineSetting("PostItWindowize", PostItSize.Medium, () => "Post-It Note Size", () => "Sets the size of the Post-It window");
            _settingPostItOpacity = settings.DefineSetting("PostItOpacity", 80.0f, () => "Post-It Opacity (unfocused)", () => "Percentage of how Opaque/Transparent the Post It Notes window should be when not focused");
            _settingPostItOpacityFocused = settings.DefineSetting("PostItOpacityFocused", 100.0f, () => "Post-It Opacity (focused)", () => "Percentage of how Opaque/Transparent the Post It Notes window should be when focused");
            _settingPostItAlwaysOnTop = settings.DefineSetting("PostItAlwaysOnTop", false, () => "Post-It Always On Top", () => "When Checked will cause the Post-It window to be on top of any other windows");
            _settingPostItFontSize = settings.DefineSetting("PostItFontSize", "16", () => "Post-It Window Font Size", () => "the font size for the Post-It window");

            _settingShowPostItWindow.SettingChanged += UpdatePostItSettings;
            _settingPostItOpacity.SettingChanged += UpdatePostItSettings;
            _settingPostItOpacityFocused.SettingChanged += UpdatePostItSettings;
            _settingPostItAlwaysOnTop.SettingChanged += UpdatePostItSettings;
            _settingPostItSize.SettingChanged += UpdatePostItSettings;
            _settingPostItFontSize.SettingChanged += UpdatePostItSettings;
        }

        public void Initialize()
        {

            PostItFile = "_PostIt";
            PostItText = FileManager.ReadFile(PostItFile);

        }

        public async Task LoadAsync()
        {
            buildPostItSettingsPanel();
        }

        public void OnModuleLoaded()
        {

            CreatePostIt();
        }
        public void Unload()
        {

            _settingShowPostItWindow.SettingChanged -= UpdatePostItSettings;
            _settingPostItOpacity.SettingChanged -= UpdatePostItSettings;
            _settingPostItOpacityFocused.SettingChanged -= UpdatePostItSettings;
            _settingPostItAlwaysOnTop.SettingChanged -= UpdatePostItSettings;
            _settingPostItSize.SettingChanged -= UpdatePostItSettings;

            _postItWindow.Dispose();
        }
        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<PostItSize> e = null)
        {
            CreatePostIt();
        }
        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<String> e = null)
        {
            CreatePostIt();

        }
        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<float> e = null)
        {
            assignPostItOpacity(false, false);
        }

        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
        {
            if (_settingShowPostItWindow.Value)
            {
                CreatePostIt();
            }
            else
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
                Font = getFont(_settingPostItFontSize.Value),
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
                if (postItTextBox.Focused == false)
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
            }
            else
            {
                _postItWindow.Opacity = opacityUnfocused;
            }
        }
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

        public void Show()
        {
            _postItWindow.Show();
        }
        public void ToggleWindow()
        {
            _postItWindow.ToggleWindow();
        }

        private BitmapFont getFont(string size)
        {
            BitmapFont font = GameService.Content.DefaultFont16;

            switch (_settingPostItFontSize.Value)
            {
                case "12":
                    font = GameService.Content.DefaultFont12;
                    break;
                case "14":
                    font = GameService.Content.DefaultFont14;
                    break;
                case "16":
                    font = GameService.Content.DefaultFont16;
                    break;
                case "18":
                    font = GameService.Content.DefaultFont18;
                    break;
                case "32":
                    font = GameService.Content.DefaultFont32;
                    break;
                default:
                    font = GameService.Content.DefaultFont16;
                    break;
            }
            return font;
        }

        private Panel buildPostItSettingsPanel()
        {
            Panel postItPanel = new Panel()
            {
                CanScroll = false,
                HeightSizingMode = SizingMode.AutoSize,
                Title = "Post-It Settings"
            };

            IView settingShowPostItWindow_View = SettingView.FromType(_settingShowPostItWindow, postItPanel.Width);
            ViewContainer settingShowPostItWindow_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, 10),
                Parent = postItPanel
            };
            settingShowPostItWindow_Container.Show(settingShowPostItWindow_View);

            IView settingPostItAlwaysOnTop_View = SettingView.FromType(_settingPostItAlwaysOnTop, postItPanel.Width);
            ViewContainer settingPostItAlwaysOnTop_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(165, 10),
                Parent = postItPanel
            };
            settingPostItAlwaysOnTop_Container.Show(settingPostItAlwaysOnTop_View);

            IView settingPostItSize_View = SettingView.FromType(_settingPostItSize, postItPanel.Width);
            ViewContainer settingPostItSize_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingPostItAlwaysOnTop_Container.Bottom + 10),
                Parent = postItPanel
            };
            settingPostItSize_Container.Show(settingPostItSize_View);


            Label settingPostItFontSize_Label = new Label()
            {
                Location = new Point(15, settingPostItSize_Container.Bottom + 13),
                Width = 100,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = postItPanel,
                Text = "Post-It Font Size ",
            };
            Dropdown settingPostItFontSize_Select = new Dropdown()
            {
                Location = new Point(settingPostItFontSize_Label.Right + 8, settingPostItFontSize_Label.Top - 4),
                Width = 60,
                Parent = postItPanel,
            };
            foreach (var s in _fontSizes)
            {
                settingPostItFontSize_Select.Items.Add(s);
            }
            settingPostItFontSize_Select.SelectedItem = _settingPostItFontSize.Value;
            settingPostItFontSize_Select.ValueChanged += delegate {
                _settingPostItFontSize.Value = settingPostItFontSize_Select.SelectedItem;
            };
            


            IView settingPostItOpacity_View = SettingView.FromType(_settingPostItOpacity, postItPanel.Width);
            ViewContainer settingPostItOpacity_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingPostItFontSize_Select.Bottom + 7),
                Parent = postItPanel
            };
            settingPostItOpacity_Container.Show(settingPostItOpacity_View);

            IView settingPostItOpacityFocused_View = SettingView.FromType(_settingPostItOpacityFocused, postItPanel.Width);
            ViewContainer settingPostItOpacityFocused_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingPostItOpacity_Container.Bottom + 10),
                Parent = postItPanel
            };
            settingPostItOpacityFocused_Container.Show(settingPostItOpacityFocused_View);

            _postItSettingsPanel = postItPanel;
            return postItPanel;
            /*
    internal SettingEntry<string> _settingPostItFontSize;*/
        }
        public Panel getPostItSettingPanel(Container parentpanel)
        {
            if(_postItSettingsPanel == null) { buildPostItSettingsPanel(); }
            _postItSettingsPanel.Parent = parentpanel;
            _postItSettingsPanel.Width = parentpanel.Width;
            return _postItSettingsPanel;
        }
    }
}

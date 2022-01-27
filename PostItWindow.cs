using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Overlay.UI.Views;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using Snaid1.Blishpad.Utility;
using Snaid1.Blishpad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad
{
    public class PostItWindow
    {
        internal SettingsManager SettingsManager;
        internal ContentsManager ContentsManager;
        internal DirectoriesManager DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager;

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
        internal SettingEntry<bool> _settingEscClosesPostIt;
        internal SettingEntry<bool> _settingPreservePostItContents;
        internal SettingEntry<KeyBinding> _settingPostItToggleKey;

        private StandardWindow _postItWindow { get; set; }
        private MultilineTextBox _postItTextBox;
        private Panel _postItSettingsPanel;
        


        private string PostItText;
        private string PostItFile;
        public void SetManagers(SettingsManager sm, ContentsManager cm, DirectoriesManager dm, Gw2ApiManager apim)
        {
            SettingsManager = sm;
            ContentsManager = cm;
            DirectoriesManager = dm;
            Gw2ApiManager = apim;
        }
        public void DefineSettings(SettingCollection settings)
        {
            _settingShowPostItWindow = settings.DefineSetting("ShowPostItWindow", true, () => "Enable Post-It Note", () => "Enables a notes window that can be kept On Screen");
            _settingPostItSize = settings.DefineSetting("PostItWindowize", PostItSize.Medium, () => "Post-It Note Size", () => "Sets the size of the Post-It window");
            _settingPostItOpacity = settings.DefineSetting("PostItOpacity", 80.0f, () => "Post-It Opacity (unfocused)", () => "Percentage of how Opaque/Transparent the Post It Notes window should be when not focused");
            _settingPostItOpacityFocused = settings.DefineSetting("PostItOpacityFocused", 100.0f, () => "Post-It Opacity (focused)", () => "Percentage of how Opaque/Transparent the Post It Notes window should be when focused");
            _settingPostItAlwaysOnTop = settings.DefineSetting("PostItAlwaysOnTop", false, () => "Post-It Always On Top", () => "When Checked will cause the Post-It window to be on top of any other windows");
            _settingPostItFontSize = settings.DefineSetting("PostItFontSize", "16", () => "Post-It Window Font Size", () => "the font size for the Post-It window");
            _settingEscClosesPostIt = settings.DefineSetting("PostItEscClosesIt", false, () => "Close Post-It with Esc", () => "Allows Post-It window to be closed by pressing Esc or clicking the close 'x'");
            _settingPreservePostItContents = settings.DefineSetting("PostItPreserveContents", true, () => "Preserve Post-It Contents", () => "Causes the Post-It window to remember it's content's between runs.");
            _settingPostItToggleKey = settings.DefineSetting("PostItToggleHotkey", new KeyBinding(ModifierKeys.Ctrl, Keys.P) { Enabled = true, BlockSequenceFromGw2 = true }, () => "Toggle Post-It Hotkey", () => "Hotkey used to toggle the Post-It pad on or off");

            _settingShowPostItWindow.SettingChanged += UpdatePostItSettings;
            _settingPostItOpacity.SettingChanged += UpdatePostItSettings;
            _settingPostItOpacityFocused.SettingChanged += UpdatePostItSettings;
            _settingPostItAlwaysOnTop.SettingChanged += UpdateAlwaysOnTop;
            _settingPostItSize.SettingChanged += UpdatePostItSettings;
            _settingPostItFontSize.SettingChanged += UpdatePostItSettings;
            _settingEscClosesPostIt.SettingChanged += UpdatePostItClosable;
        }

        public void Initialize()
        {

            PostItFile = "_PostIt";
            PostItText = "";
            if (_settingPreservePostItContents.Value) { PostItText = FileHelper.ReadFile(PostItFile); }

        }

        public async Task LoadAsync()
        {
            _postItSettingsPanel = new PostItSettingView(this).buildPostItSettingsPanel();
        }

        public void OnModuleLoaded()
        {
            CreatePostIt();
            SetPostItHotkey();
        }
        public void Unload()
        {

            _settingShowPostItWindow.SettingChanged -= UpdatePostItSettings;
            _settingPostItOpacity.SettingChanged -= UpdatePostItSettings;
            _settingPostItOpacityFocused.SettingChanged -= UpdatePostItSettings;
            _settingPostItAlwaysOnTop.SettingChanged -= UpdateAlwaysOnTop;
            _settingPostItSize.SettingChanged -= UpdatePostItSettings;
            _settingPostItFontSize.SettingChanged -= UpdatePostItSettings;
            _settingEscClosesPostIt.SettingChanged -= UpdatePostItClosable;

            _postItTextBox?.Dispose();
            _postItWindow?.Dispose();
            _postItSettingsPanel?.Dispose();
        }
        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<PostItSize> e = null)
        {
            CreatePostIt();
        }
        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<String> e = null)
        {
            if(_postItWindow != null){
                _postItTextBox.Font = getFont(_settingPostItFontSize.Value);
            }

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
        private void UpdateAlwaysOnTop(object sender = null, ValueChangedEventArgs<bool> e = null)
        {
            if (_postItWindow != null)
            {
                _postItWindow.TopMost = _settingPostItAlwaysOnTop.Value;
            }
        }
        private void UpdatePostItClosable(object sender = null, ValueChangedEventArgs<bool> e = null)
        {
            if (_postItWindow != null)
            {
                _postItWindow.CanClose = _settingEscClosesPostIt.Value;
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

            windowRec = BlishpadUtility.ScaleRectangle(windowRec, scale);
            contentRec = BlishpadUtility.ScaleRectangle(contentRec, scale);

            _postItWindow = new StandardWindow(pageTexture, windowRec, contentRec)
            {
                Parent = Blish_HUD.GameService.Graphics.SpriteScreen,
                SavesPosition = true,
                Title = "Post-It",
                Id = $"{nameof(NotesModule)}_NotesModulePostIt_38d38590-b5f9-447d-97ea-37b0b50e5e56",
                TopMost = _settingPostItAlwaysOnTop.Value,
                Opacity = _settingPostItOpacity.Value / 100,
                CanResize = false,
                CanClose = _settingEscClosesPostIt.Value,
                HeightSizingMode = SizingMode.Standard,
                WidthSizingMode = SizingMode.Standard
            };
            _postItTextBox = new MultilineTextBox()
            {
                Parent = _postItWindow,
                PlaceholderText = "Enter notes here ...",
                Size = BlishpadUtility.ScalePoint(new Point(358, 500), scale),
                Font = getFont(_settingPostItFontSize.Value),
                Location = new Point(0, 0),
                Text = PostItText

            };

            _postItWindow.MouseEntered += delegate { mouseOn = true; assignPostItOpacity(mouseOn, _postItTextBox.Focused); };
            _postItWindow.MouseLeft += delegate {
                mouseOn = false;
                assignPostItOpacity(mouseOn, _postItTextBox.Focused);
            };

            _postItTextBox.InputFocusChanged += delegate
            {
                assignPostItOpacity(mouseOn, _postItTextBox.Focused);
                if (_postItTextBox.Focused == false && _settingPreservePostItContents.Value)
                {
                    FileHelper.WriteFile(PostItFile, _postItTextBox.Text);
                }
            };
            


            if (_settingShowPostItWindow.Value)
            {
                _postItWindow.Show();
            }
            assignPostItOpacity(mouseOn, _postItTextBox.Focused);
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

        
        public Panel getPostItSettingPanel(Container parentpanel)
        {
            if(_postItSettingsPanel == null) { _postItSettingsPanel = new PostItSettingView(this).buildPostItSettingsPanel(); }
            _postItSettingsPanel.Parent = parentpanel;
            _postItSettingsPanel.Width = parentpanel.Width;
            return _postItSettingsPanel;
        }

        public void SetPostItHotkey()
        {
            _settingPostItToggleKey.Value.Activated += PostItToggleHotkey_Activated;
        }

        private void PostItToggleHotkey_Activated(object sender, EventArgs e)
        {
            if(_postItWindow != null)
            {
                _postItWindow.ToggleWindow();
            }
        }
    }
}

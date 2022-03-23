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
using Snaid1.Blishpad.Controls;
using Snaid1.Blishpad.Types;
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
        public ContentsManager ContentsManager;

        public enum PostItSize
        {
            XSmall,
            Small,
            Medium,
            Large,
            XLarge
        }

        private StandardWindow _postItWindow { get; set; }
        private NotesMultilineTextBox _postItTextBox;

        private KeyBinding escKey = new KeyBinding(Keys.Escape) { Enabled = false, BlockSequenceFromGw2 = true };

        private string PostItText;
        private string PostItFile;
        public void DefineSettings(SettingCollection settings)
        {
            NotesModule._settingShowPostItWindow = settings.DefineSetting("ShowPostItWindow", true, () => "Enable Post-It Note", () => "Enables a notes window that can be kept On Screen");
            NotesModule._settingPostItSize = settings.DefineSetting("PostItWindowize", PostItSize.Medium, () => "Post-It Note Size", () => "Sets the size of the Post-It window");
            NotesModule._settingPostItOpacity = settings.DefineSetting("PostItOpacity", 80.0f, () => "Post-It Opacity (unfocused)", () => "Percentage of how Opaque/Transparent the Post It Notes window should be when not focused");
            NotesModule._settingPostItOpacityFocused = settings.DefineSetting("PostItOpacityFocused", 100.0f, () => "Post-It Opacity (focused)", () => "Percentage of how Opaque/Transparent the Post It Notes window should be when focused");
            NotesModule._settingPostItAlwaysOnTop = settings.DefineSetting("PostItAlwaysOnTop", false, () => "Post-It Always On Top", () => "When Checked will cause the Post-It window to be on top of any other windows");
            NotesModule._settingPostItFontSize = settings.DefineSetting("PostItFontSize", "16", () => "Post-It Font Size", () => "The font size for the Post-It window");
            NotesModule._settingEscClosesPostIt = settings.DefineSetting("PostItEscClosesIt", true, () => "Close Post-It with Esc", () => "Allows Post-It window to be closed by pressing Esc or clicking the close 'x'");
            NotesModule._settingPreservePostItContents = settings.DefineSetting("PostItPreserveContents", true, () => "Preserve Post-It Contents", () => "Causes the Post-It window to remember it's content's between runs.");
            NotesModule._settingPostItToggleKey = settings.DefineSetting("PostItToggleHotkey", new KeyBinding(ModifierKeys.Ctrl, Keys.P) { Enabled = true, BlockSequenceFromGw2 = true }, () => "Toggle Post-It Hotkey", () => "Hotkey used to toggle the Post-It pad on or off");

            NotesModule._settingShowPostItWindow.SettingChanged += UpdatePostItSettings;
            NotesModule._settingPostItOpacity.SettingChanged += UpdatePostItSettings;
            NotesModule._settingPostItOpacityFocused.SettingChanged += UpdatePostItSettings;
            NotesModule._settingPostItAlwaysOnTop.SettingChanged += UpdateAlwaysOnTop;
            NotesModule._settingPostItSize.SettingChanged += UpdatePostItSize;
            NotesModule._settingPostItFontSize.SettingChanged += UpdatePostItFontSize;
            //NotesModule._settingEscClosesPostIt.SettingChanged += UpdatePostItClosable;

            escKey.Activated += delegate
            {
                _postItWindow.Hide();
                escKey.Enabled = false;
                NotesModule._settingShowPostItWindow.Value = false;
            };
        }

        public void Initialize()
        {

            PostItFile = "_PostIt";
            PostItText = "";

        }

        public async Task LoadAsync()
        {
            if (NotesModule._settingPreservePostItContents.Value) { PostItText = FileHelper.ReadFile(PostItFile); }
        }

        public void OnModuleLoaded()
        {
            CreatePostIt();
            SetPostItHotkey();
        }
        public void Unload()
        {

            NotesModule._settingShowPostItWindow.SettingChanged -= UpdatePostItSettings;
            NotesModule._settingPostItOpacity.SettingChanged -= UpdatePostItSettings;
            NotesModule._settingPostItOpacityFocused.SettingChanged -= UpdatePostItSettings;
            NotesModule._settingPostItAlwaysOnTop.SettingChanged -= UpdateAlwaysOnTop;
            NotesModule._settingPostItSize.SettingChanged -= UpdatePostItSize;
            NotesModule._settingPostItFontSize.SettingChanged -= UpdatePostItFontSize;

            _postItTextBox?.Dispose();
            _postItWindow?.Dispose();
        }
        private void UpdatePostItSize(object sender = null, ValueChangedEventArgs<PostItSize> e = null)
        {
            CreatePostIt();
        }
        private void UpdatePostItFontSize(object sender = null, ValueChangedEventArgs<String> e = null)
        {
            if(_postItWindow != null){
                _postItTextBox.Font = BlishpadUtility.getFont(NotesModule._settingPostItFontSize.Value);
            }

        }
        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<float> e = null)
        {
            assignPostItOpacity(false, false);
        }

        private void UpdatePostItSettings(object sender = null, ValueChangedEventArgs<bool> e = null)
        {
            if (NotesModule._settingShowPostItWindow.Value)
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
                _postItWindow.TopMost = NotesModule._settingPostItAlwaysOnTop.Value;
            }
        }
        private void UpdatePostItClosable(object sender = null, ValueChangedEventArgs<bool> e = null)
        {
            if (_postItWindow != null)
            {
                _postItWindow.CanClose = NotesModule._settingEscClosesPostIt.Value;
            }
        }

        private void CreatePostIt()
        {
            _postItWindow?.Dispose();
            bool mouseOn = false;
            float scale = 1f;
            Texture2D pageTexture;

            switch (NotesModule._settingPostItSize.Value)
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
                TopMost = NotesModule._settingPostItAlwaysOnTop.Value,
                Opacity = NotesModule._settingPostItOpacity.Value / 100,
                CanResize = false,
                CanClose = false,
                HeightSizingMode = SizingMode.Standard,
                WidthSizingMode = SizingMode.Standard
            };

            _postItTextBox = new NotesMultilineTextBox(new Types.NotesFile(PostItFile, null))
            {
                Parent = _postItWindow,
                PlaceholderText = "Enter notes here ...",
                Size = BlishpadUtility.ScalePoint(new Point(358, 500), scale),
                Font = BlishpadUtility.getFont(NotesModule._settingPostItFontSize.Value),
                Location = new Point(0, 0),
                Text = PostItText
            };
            _postItTextBox.TextChanged += delegate { PostItText = _postItTextBox.Text; };

            _postItWindow.MouseEntered += delegate { mouseOn = true; assignPostItOpacity(mouseOn, _postItTextBox.Focused); };
            _postItWindow.MouseLeft += delegate {
                mouseOn = false;
                assignPostItOpacity(mouseOn, _postItTextBox.Focused);
            };

            _postItTextBox.InputFocusChanged += delegate
            {
                assignPostItOpacity(mouseOn, _postItTextBox.Focused);
                if (_postItTextBox.Focused == false && NotesModule._settingPreservePostItContents.Value)
                {
                    FileHelper.WriteFile(PostItFile, _postItTextBox.Text);
                }
            };
            

            if (NotesModule._settingShowPostItWindow.Value)
            {
                _postItWindow.Show();
            }
            assignPostItOpacity(mouseOn, _postItTextBox.Focused);
        }

        private void assignPostItOpacity(Boolean mouseOn, Boolean focused)
        {
            var opacityFocused = NotesModule._settingPostItOpacityFocused.Value / 100;
            var opacityUnfocused = NotesModule._settingPostItOpacity.Value / 100;

            if (focused || mouseOn)
            {
                _postItWindow.Opacity = opacityFocused;
                escKey.Enabled = true && NotesModule._settingEscClosesPostIt.Value;
            }
            else
            {
                _postItWindow.Opacity = opacityUnfocused;
                escKey.Enabled = false;
            }
        }

        public void Show()
        {
            NotesModule._settingShowPostItWindow.Value = true;
            _postItTextBox.Text = PostItText;
            _postItWindow.Show();
        }
        public void ToggleWindow()
        {
            _postItWindow.ToggleWindow();
            if(_postItWindow.Visible == false)
            {
                PostItText = _postItTextBox.Text;
                if (NotesModule._settingPreservePostItContents.Value)
                {
                    _postItTextBox.Save();
                }
                escKey.Enabled = false;
            }
            NotesModule._settingShowPostItWindow.Value = (NotesModule._settingShowPostItWindow.Value == false);
        }

        public void SetPostItHotkey()
        {
            NotesModule._settingPostItToggleKey.Value.Activated += PostItToggleHotkey_Activated;
        }

        private void PostItToggleHotkey_Activated(object sender, EventArgs e)
        {
            ToggleWindow();
        }

        public void CopyToPostIt(NotesFile nf)
        {
            _postItTextBox.NoteFile = nf.clone();
        }

        public string CopyFromPostIt()
        {
            return _postItTextBox.Text;
        }
    }
}

using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Views
{

    class PostItSettingView : View
    {
        public static int totalHeight;

        public PostItSettingView(){}
        protected override void Build(Container postItPanel)
        {
            //checkboxes
            IView settingShowPostItWindow_View = SettingView.FromType(NotesModule._settingShowPostItWindow, postItPanel.Width);
            ViewContainer settingShowPostItWindow_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, 10),
                Parent = postItPanel
            };
            settingShowPostItWindow_Container.Show(settingShowPostItWindow_View);

            IView settingPostItAlwaysOnTop_View = SettingView.FromType(NotesModule._settingPostItAlwaysOnTop, postItPanel.Width);
            ViewContainer settingPostItAlwaysOnTop_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(165, 10),
                Parent = postItPanel
            };
            settingPostItAlwaysOnTop_Container.Show(settingPostItAlwaysOnTop_View);

            IView settingEscClosesPostIt_View = SettingView.FromType(NotesModule._settingEscClosesPostIt, postItPanel.Width);
            ViewContainer settingEscClosesPostIt_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(340, 10),
                Parent = postItPanel
            };
            settingEscClosesPostIt_Container.Show(settingEscClosesPostIt_View);

            IView settingPreservePostItContents_View = SettingView.FromType(NotesModule._settingPreservePostItContents, postItPanel.Width);
            ViewContainer settingPreservePostItContents_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingShowPostItWindow_Container.Bottom + 10),
                Parent = postItPanel
            };
            settingPreservePostItContents_Container.Show(settingPreservePostItContents_View);

            //Drop Down Lists
            IView settingPostItSize_View = SettingView.FromType(NotesModule._settingPostItSize, postItPanel.Width);
            ViewContainer settingPostItSize_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingPreservePostItContents_Container.Bottom + 10),
                Parent = postItPanel
            };
            settingPostItSize_Container.Show(settingPostItSize_View);

            IView settingPostItFontSize_View = new FontSizeSettingView(NotesModule._settingPostItFontSize);
            ViewContainer settingPostItFontSize_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingPostItSize_Container.Bottom + 10),
                Parent = postItPanel
            };
            settingPostItFontSize_Container.Show(settingPostItFontSize_View);


            //Sliders
            IView settingPostItOpacity_View = SettingView.FromType(NotesModule._settingPostItOpacity, postItPanel.Width);
            ViewContainer settingPostItOpacity_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingPostItFontSize_Container.Bottom + 15),
                Parent = postItPanel
            };
            settingPostItOpacity_Container.Show(settingPostItOpacity_View);

            IView settingPostItOpacityFocused_View = SettingView.FromType(NotesModule._settingPostItOpacityFocused, postItPanel.Width);
            ViewContainer settingPostItOpacityFocused_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(10, settingPostItOpacity_Container.Bottom + 10),
                Parent = postItPanel
            };
            settingPostItOpacityFocused_Container.Show(settingPostItOpacityFocused_View);

            //Hotkey
            IView settingPostItToggleKey_View = SettingView.FromType(NotesModule._settingPostItToggleKey, postItPanel.Width);
            ViewContainer settingPostItToggleKey_Container = new ViewContainer()
            {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(15, settingPostItOpacityFocused_Container.Bottom + 10),
                Parent = postItPanel
            };
            settingPostItToggleKey_Container.Show(settingPostItToggleKey_View);

            totalHeight = settingPostItToggleKey_Container.Bottom;
        }

        public Panel BuildPostItSettingsPanel()
        {
            Panel postItPanel = new Panel()
            {
                CanScroll = false,
                HeightSizingMode = SizingMode.AutoSize,
                Title = "Post-It Settings"
            };
            
            Build(postItPanel);
            return postItPanel;
        }
        public Panel BuildPostItSettingsPanel(Container parentPanel)
        {
            Panel postItPanel = BuildPostItSettingsPanel();

            postItPanel.Parent = parentPanel;
            postItPanel.Width = parentPanel.Width;
            
            return postItPanel;
        }
    }
}

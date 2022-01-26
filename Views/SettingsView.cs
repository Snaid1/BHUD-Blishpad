using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Blish_HUD.Settings.UI.Views;
using Blish_HUD.Graphics.UI;

namespace Snaid1.Blishpad.Views
{
    class SettingsView : View
    {
        private NotesModule module;
        public SettingsView(NotesModule notesModule)
        {
            module = notesModule;
        }
        protected override void Build(Container buildPanel)
        {
            Panel parentPanel = new Panel()
            {
                CanScroll = false,
                Parent = buildPanel,
                Height = buildPanel.Height,
                Width = buildPanel.Width - 20
            };

            var PostItPanel = NotesModule.getPostIt().getPostItSettingPanel(parentPanel);
            if (buildPanel is Panel)
            {
                PostItPanel.ShowBorder = false;
                PostItPanel.Title = "";
            }
            
            var notesPanel = module.GetNotesManagerSettingView(parentPanel);
            notesPanel.Top = PostItPanel.Top + calcHeight(PostItPanel,0)-40;
            if (buildPanel is Panel)
            {
                notesPanel.ShowBorder = false;
                notesPanel.Title = "";
            }
        }

        private int calcHeight(Control cont, int max)
        {
            var newmax = max;
            if(newmax < cont.Height)
            {
                newmax = cont.Height;
            }
            if(cont is Container)
            {
                Container container = (Container)cont;
                var childrenHeight = 0;
                foreach(Control child in container.Children)
                {
                    childrenHeight += calcHeight(child, newmax);
                }
                if(newmax < childrenHeight) { newmax = childrenHeight; }
            }
            return newmax;
        }
    }
}

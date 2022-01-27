using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Snaid1.Blishpad.Controls;
using Snaid1.Blishpad.Types;
using Snaid1.Blishpad.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Views
{
    class NotesWindowView : View
    {
        private NotesWindow notesWindow;
        private NotesManager notesManager;
        private List<NotesFile> notesFiles;

        public NotesWindowView(NotesWindow nwin)
        {
            notesWindow = nwin;
            notesManager = notesWindow.getNotesManager();
        }
        protected override void Build(Container buildPanel)
        {
            FlowPanel contents = new FlowPanel()
            {
                Parent = buildPanel,
                Height = buildPanel.Height,
                Width = buildPanel.Width
            };
            BuildFileList(contents);
        }
        protected void BuildFileList(Container buildPanel)
        {
            Panel filesPanel = new Panel()
            {
                CanScroll = true,
                HeightSizingMode = SizingMode.AutoSize,
                Width = BlishpadUtility.ScaleInt(buildPanel.Width,0.3f),
                Title = "Notes",
                Parent = buildPanel,
                Left = 30,
                ShowBorder = true
            };

            Menu filesMenu = new Menu()
            {
                Parent = filesPanel,
                HeightSizingMode = SizingMode.AutoSize,
                Width = filesPanel.Width,
            };

            MultilineTextBox contentsBox = new MultilineTextBox()
            {
                Parent = buildPanel,
                Height = buildPanel.Height,
                Width = BlishpadUtility.ScaleInt(buildPanel.Width, 0.7f),
            };

            FetchFileMenuItems(filesMenu, contentsBox);
        }

        protected List<NotesFile> FetchFileMenuItems(Menu menu, MultilineTextBox contentsbox)
        {
            notesFiles = new List<NotesFile>();
            String[] filenames = FileHelper.GetAllFilesInNotesDir();
            foreach(string fname in filenames)
            {
                var filename = Path.GetFileName(fname);
                NotesFile fileDetails = new NotesFile(filename, contentsbox);
                fileDetails.MenuItem.Parent = menu;
                notesFiles.Add(fileDetails);
            }
            return notesFiles;
        }
    }    
}

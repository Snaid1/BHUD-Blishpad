using Blish_HUD.Controls;
using Snaid1.Blishpad.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Types
{
    class NotesFile
    {
        private string filename;
        private string contents;
        private MenuItem menuItem;
        private MultilineTextBox ContentTarget;

        public string FileName
        {
            get => filename;
            set
            {
                filename = FileHelper.SanitizeFileName( value );
            }
        }
        public string Title { get { return FileHelper.StripExtension(filename); } }
        public string Contents { get { return contents; } }
        public MenuItem MenuItem { get { return menuItem; } }


        public NotesFile(string fname, MultilineTextBox contentTarget)
        {
            this.FileName = fname;
            this.ContentTarget = contentTarget;
            this.contents = readFile();
            BuildMenuItem();

            menuItem.ItemSelected += HandleItemSelected;
        }

        private void HandleItemSelected(object sender, ControlActivatedEventArgs e)
        {
            ContentTarget.Text = Contents;
        }

        private MenuItem BuildMenuItem()
        {
            menuItem = new MenuItem(Title);
            return menuItem;
        }

        public String readFile()
        {
            return FileHelper.ReadFile(FileName);
        }
    }
}

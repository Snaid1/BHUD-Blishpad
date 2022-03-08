using Blish_HUD.Controls;
using Blish_HUD.Input;
using Snaid1.Blishpad.Controls;
using Snaid1.Blishpad.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Types
{
    public class NotesFile
    {
        private string filename;
        private string contents;
        private MenuItem menuItem;
        private NotesMultilineTextBox ContentTarget;

        public string FileName
        {
            get => filename;
            set
            {
                filename = FileHelper.SanitizeFileName( value );
            }
        }
        public string Title { get { return FileHelper.StripExtension(filename); } }
        public string Contents { get { return contents; } set { contents = value;  } }
        public MenuItem MenuItem { get { return menuItem; } }


        public NotesFile(string fname, NotesMultilineTextBox contentTarget)
        {
            this.FileName = fname;
            this.ContentTarget = contentTarget;
            this.contents = readFile();
            BuildMenuItem();
            if(contentTarget != null)
            {
                menuItem.Click += HandleItemSelected;
            }
        }

        public NotesFile(NotesFile nf)
        {
            this.FileName = nf.filename;
            this.Contents = nf.contents;
            this.menuItem = new MenuItem(nf.Title);
            this.ContentTarget = nf.ContentTarget;
            if (this.ContentTarget != null)
            {
                menuItem.Click += HandleItemSelected;
            }
        }

        public void HandleItemSelected(object sender, MouseEventArgs e)
        {
            ContentTarget.NoteFile = this;
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

        public void saveFile()
        {
            FileHelper.WriteFile(this.FileName, this.Contents);
        }

        public NotesFile clone()
        {
            return new NotesFile(this.filename, null)
            {
                contents = this.contents,
                menuItem = new MenuItem(this.Title)
            };
        }

        public void DeleteFile()
        {
            FileHelper.DeleteFile(this.FileName);
            this.Unload();
        }

        public void Unload()
        {
            this.MenuItem.Dispose();
        }
    }
}

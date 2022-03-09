using Blish_HUD;
using Blish_HUD.Controls;
using Snaid1.Blishpad.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Controls
{
    public class NotesMultilineTextBox : MultilineTextBox
    {
        private NotesFile _noteFile;
        public NotesFile NoteFile
        {
            get => _noteFile;
            set
            {
                _noteFile = value;
                this.Text = _noteFile.Contents;
            }
        }

        public NotesMultilineTextBox() : base()
        {
            this.TextChanged += delegate
            {
                if(_noteFile != null)
                {
                    _noteFile.Contents = this.Text;
                }
            };
        }
        public NotesMultilineTextBox(NotesFile nf) : base()
        {
            NoteFile = nf;
            this.TextChanged += delegate
            {
                _noteFile.Contents = this.Text;
            };
        }


        public void Save()
        {
            this._noteFile.saveFile();
        }
        public void Reload()
        {
            this.Text = this._noteFile.readFile();
        }

        public void Clear()
        {
            this._noteFile.Contents = "";
            this.Text = "";
        }

        public void DeleteNote()
        {
            this._noteFile.DeleteFile();
            this._noteFile = null;
            this.Text = "";
        }
    }
}

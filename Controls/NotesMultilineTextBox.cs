using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using MonoGame.Extended.BitmapFonts;
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
        protected static readonly char[] ChatCodeStartSymbols = { '&', '=', '+', '[', '/' };
        protected static readonly char[] ChatCodeEndSymbols = { '&', '=', '+', ']', '/' };

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

        protected override void OnClick(MouseEventArgs e)
        {
            base.OnClick(e);

            this.Focused = true;
            if (e.IsDoubleClick)
            {
                HandleSelectChatCodeAndCopyToClipboard(GetCursorIndexFromPosition(this.RelativeMousePosition));
            }
        }

        protected void HandleSelectChatCodeAndCopyToClipboard(int newIndex)
        {
            if (_cursorIndex == newIndex)
            {
                int startindex = GetStartOfChatCode(newIndex);
                if(startindex == -1) { return; }
                int endindex = GetEndOfChatCode(newIndex);
                if(endindex == -1) { return; }
                if(endindex > startindex)
                {
                    this.SelectionStart = startindex;
                    this.SelectionEnd = endindex;
                    CopyToClipboardAsync(_text.Substring(startindex, endindex - startindex));
                }
            }
        }

        protected int GetStartOfChatCode(int index)
        {
            if(index > 0 && index < _text.Length)
            {
                while (index > 0 && (index - 1 >= _text.Length || !WordSeperators.Contains(_text[index - 1]) || ChatCodeStartSymbols.Contains(_text[index - 1])) && _text[index] != '[')
                {
                    index--;
                }
                if (_text[index] != '[' || _text[index + 1] != '&') { index = -1; }
            } else
            {
                index = -1;
            }
            return index;
        }

        protected int GetEndOfChatCode(int index)
        {
            if(index > 0 && index < _text.Length)
            {
                while (index < _text.Length && (!WordSeperators.Contains(_text[index]) || ChatCodeEndSymbols.Contains(_text[index])) && _text[index - 1] != ']')
                {
                    index++;
                }
                if (_text[index-1] != ']') { index = -1; }
            } else
            {
                index = -1;
            }
            return index;
        }

        protected async Task CopyToClipboardAsync(string textToCopy)
        {
            if (textToCopy != null)
            {
                var clipboardResult = await ClipboardUtil.WindowsClipboardService.SetTextAsync(textToCopy);

                if (!clipboardResult)
                {
                    ScreenNotification.ShowNotification("Failed to copy chat link to clipboard.Try again.", ScreenNotification.NotificationType.Red, duration: 2);
                }
                else
                {
                    if (NotesModule._settingShowNotificationOnCopy.Value)
                    {
                        ScreenNotification.ShowNotification("Chat link copied to clipboard", duration: 2);
                    }
                }
            }
        }
    }
}

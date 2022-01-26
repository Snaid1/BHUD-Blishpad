using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Snaid1.Blishpad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Views
{
    class NotesWindowView : View
    {
        private NotesWindow notesWindow;

        public NotesWindowView(NotesWindow nwin)
        {
            notesWindow = nwin;
        }
        protected override void Build(Container buildPanel)
        {

        }
    }    
}

using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Snaid1.Blishpad.Controller;
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
    class NotesTabView : View
    {
        public NotesWindow notesWindow;
        private NotesWindowController notesManager;
        private List<NotesFile> notesFiles;
        private readonly String defaultTitleText = "No Note Selected";

        private Label titleLabel;
        private NotesMultilineTextBox noteContentsBox;
        private Menu filesMenu;

        public bool NewNoteNotInProcess = true;
        private string _newNoteName;
        public string newNoteName {
            get { return _newNoteName; } 
            set {
                NewNoteNotInProcess = true;
                _newNoteName = value;
                RedrawFileMenu();
                Initialize();
                _newNoteName = "";
            } 
        }

        public NotesTabView(NotesWindow nwin)
        {
            notesWindow = nwin;
            notesManager = notesWindow.GetNotesWindowController();
        }
        protected override void Build(Container buildPanel)
        {
            FlowPanel contents = new FlowPanel()
            {
                Parent = buildPanel,
                Height = buildPanel.Height,
                Width = buildPanel.Width,
                FlowDirection = ControlFlowDirection.SingleLeftToRight
            };
            BuildFileList(contents);
            Initialize();
        }
        protected void BuildFileList(Container buildPanel)
        {
            Panel filesPanel = new Panel()
            {
                CanScroll = true,
                HeightSizingMode = SizingMode.AutoSize,
                Width = BlishpadUtility.ScaleInt(buildPanel.Width,0.3f),
                Title = "Notes Manager",
                Parent = buildPanel,
                Left = 37,
                ShowBorder = true
            };

            FlowPanel contentsPanel = new FlowPanel()
            {
                CanScroll = false,
                Parent = buildPanel,
                Width = BlishpadUtility.ScaleInt(buildPanel.Width, 0.6f),
                Height = buildPanel.Height,
                Title = null,
                ShowBorder = false,
                FlowDirection = ControlFlowDirection.SingleTopToBottom
            };

            filesMenu = new Menu()
            {
                Parent = filesPanel,
                HeightSizingMode = SizingMode.AutoSize,
                Width = filesPanel.Width
            };

            titleLabel = new Label()
            {
                Text = defaultTitleText,
                Font = Blish_HUD.ContentService.Content.DefaultFont32,
                Parent = contentsPanel,
                Height = BlishpadUtility.ScaleInt(contentsPanel.Height, 0.1f),
                Width = contentsPanel.Width
            };

            noteContentsBox = new NotesMultilineTextBox()
            {
                Parent = contentsPanel,
                Height = BlishpadUtility.ScaleInt(contentsPanel.Height, 0.75f),
                Width = contentsPanel.Width
            };

            FlowPanel buttonsPanel = new FlowPanel()
            {
                Parent = contentsPanel,
                //HeightSizingMode = SizingMode.AutoSize,
                Height = BlishpadUtility.ScaleInt(contentsPanel.Height, 0.15f),
                Width = contentsPanel.Width,
                FlowDirection = ControlFlowDirection.LeftToRight,
                Title = null,
                ShowBorder = false,
                CanScroll = false
            };

            StandardButton saveButton = new StandardButton()
            {
                Parent = buttonsPanel,
                Width = BlishpadUtility.ScaleInt(buttonsPanel.Width, 0.33f),
                Text = "Save Note"
            };
            saveButton.Click += delegate { noteContentsBox.Save(); };

            StandardButton reloadButton = new StandardButton()
            {
                Parent = buttonsPanel,
                Width = BlishpadUtility.ScaleInt(buttonsPanel.Width, 0.33f),
                Text = "Reload Note"
            };
            reloadButton.Click += delegate { noteContentsBox.Reload(); };

            FetchFileMenuItems(filesMenu, noteContentsBox, titleLabel);
            
            StandardButton copyToPostItButton = new StandardButton()
            {
                Parent = buttonsPanel,
                Width = BlishpadUtility.ScaleInt(buttonsPanel.Width, 0.33f),
                Text = "Copy to Post-It -->"
            };
            copyToPostItButton.Click += delegate { NotesModule.getPostIt().CopyToPostIt(noteContentsBox.NoteFile); };

            StandardButton clearButton = new StandardButton()
            {
                Parent = buttonsPanel,
                Width = BlishpadUtility.ScaleInt(buttonsPanel.Width, 0.33f),
                Text = "Clear Note"
            };
            clearButton.Click += delegate { noteContentsBox.Clear(); };

            StandardButton deleteButton = new StandardButton()
            {
                Parent = buttonsPanel,
                Width = BlishpadUtility.ScaleInt(buttonsPanel.Width, 0.33f),
                Text = "Delete Note"
            };
            deleteButton.Click += delegate {
                notesFiles.Remove(noteContentsBox.NoteFile);
                filesMenu.RemoveChild(noteContentsBox.NoteFile.MenuItem);
                noteContentsBox.DeleteNote();
                RedrawFileMenu();
                Initialize();
            };

            StandardButton copyFromPostItButton = new StandardButton()
            {
                Parent = buttonsPanel,
                Width = BlishpadUtility.ScaleInt(buttonsPanel.Width, 0.33f),
                Text = "Copy from Post-It <--"
            };
            copyFromPostItButton.Click += delegate
            {
                String textcontents = NotesModule.getPostIt().CopyFromPostIt();
                noteContentsBox.NoteFile.Contents = textcontents;
                noteContentsBox.Text = textcontents;
            };

        }

        protected List<NotesFile> FetchFileMenuItems(Menu menu, NotesMultilineTextBox contentsbox, Label titleLabel)
        {
            CreateAddNewMenuItem(menu);
            notesFiles = new List<NotesFile>();
            String[] filenames = FileHelper.GetAllFilesInNotesDir();
            foreach(string fname in filenames)
            {
                var filename = Path.GetFileName(fname);
                if(filename == "_PostIt.txt") { continue; }
                NotesFile fileDetails = new NotesFile(filename, contentsbox);
                fileDetails.MenuItem.Parent = menu;
                fileDetails.MenuItem.ItemSelected += delegate
                {
                    titleLabel.Text = fileDetails.Title;
                };
                notesFiles.Add(fileDetails);
            }
            return notesFiles;
        }

        protected void Initialize()
        {
            if(notesFiles.Count > 0)
            {
                if(newNoteName != null && newNoteName != "")
                {
                    for(int i = 0; i < notesFiles.Count(); i++)
                    {
                        if(notesFiles[i].Title == newNoteName)
                        {
                            notesFiles[i].MenuItem.Select();
                            notesFiles[i].HandleItemSelected(this, null);
                            break;
                        }
                    }
                } 
                else
                {
                    notesFiles[0].MenuItem.Select();
                    notesFiles[0].HandleItemSelected(this, null);
                }
            } else
            {
                titleLabel.Text = defaultTitleText;
                noteContentsBox.Text = "";
            }
        }

        protected void RedrawFileMenu()
        {
            Menu newFilesMenu = new Menu
            {
                Parent = filesMenu.Parent,
                HeightSizingMode = filesMenu.HeightSizingMode,
                Width = filesMenu.Width
            };

            FetchFileMenuItems(newFilesMenu, noteContentsBox, titleLabel);
            filesMenu.Dispose();
            filesMenu = newFilesMenu;
        }

        protected MenuItem CreateAddNewMenuItem(Menu menu)
        {
            MenuItem newItem = new MenuItem("Create New Note")
            {
                Parent = menu,
                //BackgroundColor = new Microsoft.Xna.Framework.Color(10, 10, 10, 100),
                BasicTooltipText = "Click here to create a new note",
                Icon = notesWindow.contentsManager.GetTexture(@"textures\155914b.png")
            };
            newItem.Click += delegate
            {
                if (NewNoteNotInProcess)
                {
                    new NoteCreationWindow(notesManager.contentsManager, this).Show();
                }
            };
            return newItem;
        }
    }    
}

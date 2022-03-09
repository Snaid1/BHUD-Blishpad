﻿using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Snaid1.Blishpad.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Controls
{
    class NoteCreationWindow
    {
        private NotesTabView parentView;
        private StandardWindow _NoteCreationWindow;
        private readonly int windowwidth = 555;
        private readonly int windowheight = 130;

        public NoteCreationWindow(ContentsManager contentsManager, NotesTabView notesTabView, String windowTitle = "New Note")
        {
            parentView = notesTabView;
            Rectangle windowRect = new Rectangle(18, 10, windowwidth, windowheight);
            Rectangle contentRect = new Rectangle(windowRect.X + 10, windowRect.Y + 25, windowwidth - 20, windowheight - 10);
            Rectangle parentBounds = parentView.notesWindow.getWindow().AbsoluteBounds;
            _NoteCreationWindow = new StandardWindow(background: contentsManager.GetTexture(@"textures\newnotebackground.png"), windowRect, contentRect)
            {
                Parent = Blish_HUD.GameService.Graphics.SpriteScreen,
                Title = windowTitle,
                CanClose = true,
                TopMost = true,
                SavesPosition = false,
                Left = parentBounds.X + (parentBounds.Width / 2) - (windowwidth / 2) + 23,
                Top = parentBounds.Y + (parentBounds.Height / 2) - (windowheight / 2)
            };
            BuildWindowView(_NoteCreationWindow);
        }

        private Rectangle CalculateWindowDimentions()
        {
            Rectangle parentBounds = parentView.notesWindow.getWindow().AbsoluteBounds;
            int x = parentBounds.X + (parentBounds.Width / 2) - (windowwidth / 2);
            int y = parentBounds.Y + (parentBounds.Height / 2) - (windowheight / 2);
            return new Rectangle(x, y, windowwidth, windowheight);
        }

        public void Show()
        {
            _NoteCreationWindow.Show();
        }

        private void BuildWindowView(Container parentCont)
        {
            FlowPanel parentPanel = new FlowPanel()
            {
                Parent = parentCont,
                Height = parentCont.Height,
                Width = parentCont.Width,
                FlowDirection = ControlFlowDirection.SingleTopToBottom
            };

            FlowPanel titlepanel = new FlowPanel()
            {
                Parent = parentPanel,
                Width = parentPanel.Width - 5,
                Height = 50,
                //Height = parentPanel.Height - buttonPanel.Height,
                FlowDirection = ControlFlowDirection.SingleLeftToRight
            };

            FlowPanel buttonPanel = new FlowPanel()
            {
                Parent = parentPanel,
                Width = parentPanel.Width - 25,
                Height = 50,
                FlowDirection = ControlFlowDirection.SingleRightToLeft
            };
            // Title Panel Contents
            Label titleLabel = new Label()
            {
                Text = "Title: ",
                Font = Blish_HUD.ContentService.Content.DefaultFont32,
                Parent = titlepanel,
                Width = 80
            };

            TextBox titleTextBox = new TextBox()
            {
                Parent = titlepanel,
                Font = Blish_HUD.ContentService.Content.DefaultFont32,
                Width = titlepanel.Width - titleLabel.Width - 22,
            };
            //Button Panel Contents
            StandardButton CancelButton = new StandardButton()
            {
                Text = "Cancel",
                Parent = buttonPanel
            };
            CancelButton.Click += delegate
            {
                _NoteCreationWindow.Hide();
                _NoteCreationWindow.Dispose();
                
            };

            StandardButton SaveButton = new StandardButton()
            {
                Text = "Save",
                Parent = buttonPanel
            };
        }
    }
}

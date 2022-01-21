using Blish_HUD.Modules.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.BlishHudNotepad
{
    class FileHelper
    {
        internal DirectoriesManager DirectoriesManager;

        private string _notesDirectory;

        public FileHelper(DirectoriesManager DirectoriesManager)
        {
            this.DirectoriesManager = DirectoriesManager;
            _notesDirectory = DirectoriesManager.GetFullDirectoryPath("notes");
        }

        private List<String> GetAllFilesInNotesDir()
        {
            List<string> notesFiles = new List<string>();
            notesFiles.AddRange(Directory.GetFiles(_notesDirectory, "*.txt", SearchOption.AllDirectories));
            return notesFiles;
        }

        public void WriteFile(string filename, string contents)
        {
            File.WriteAllText(BuildTxtPath(filename), contents);
        }

        public string ReadFile(string filename)
        {
            string fileContents = "";
            string fileLoc = BuildTxtPath(filename);
            if (File.Exists(fileLoc))
            {
                fileContents = File.ReadAllText(fileLoc);
            }
            return fileContents;
        }
        private string BuildTxtPath(string filename)
        {
            return BuildPath(filename, ".txt");
        }
        private string BuildPath(string filename, string extension)
        {
            String fileLoc = _notesDirectory + "\\" + filename;
            if(filename.Contains(extension) == false)
            {
                fileLoc += extension;
            }
            return fileLoc;
        }
    }
}

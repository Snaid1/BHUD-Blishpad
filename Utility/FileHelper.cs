using Blish_HUD.Modules.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Utility
{
    public class FileHelper
    {

        private static string _notesDirectory;
        public static string NotesDirectory { get => _notesDirectory; set { _notesDirectory = value; } }
        public static readonly string invalidCharacters = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

        public FileHelper(DirectoriesManager DirectoriesManager)
        {
            _notesDirectory = DirectoriesManager.GetFullDirectoryPath("notes");
        }

        public static string[] GetAllFilesInNotesDir()
        {
            String[] notesFiles = Directory.GetFiles(_notesDirectory, "*.txt", SearchOption.AllDirectories);
            return notesFiles;
        }

        public static void WriteFile(string filename, string contents)
        {
            filename = SanitizeFileName(filename);
            File.WriteAllText(BuildTxtPath(filename), contents);
        }

        public static string ReadFile(string filename)
        {
            filename = SanitizeFileName(filename);
            string fileContents = "";
            string fileLoc = BuildTxtPath(filename);
            if (File.Exists(fileLoc))
            {
                fileContents = File.ReadAllText(fileLoc);
            }
            return fileContents;
        }
        private static string BuildTxtPath(string filename)
        {
            return BuildPath(filename, ".txt");
        }
        private static string BuildPath(string filename, string extension)
        {
            String fileLoc = _notesDirectory + "\\" + filename;
            if(filename.Contains(extension) == false)
            {
                fileLoc += extension;
            }
            return fileLoc;
        }
        public static string SanitizeFileName(string path)
        {
            foreach (char c in invalidCharacters)
            {
                path = path.Replace(c.ToString(), "");
            }
            return path;
        }
        public static string StripExtension(string filename)
        {
            return Path.GetFileNameWithoutExtension(filename);
        }
    }
}

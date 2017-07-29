using System.IO;

namespace Fylipp.ShortcutSwap.Core {
    // ReSharper disable once InconsistentNaming
    public class DefaultIO : IIO {

        private IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

        public void DeleteFile(string filePath) => File.Delete(filePath);

        public string[] GetDirectoriesInDirectory(string directoryPath) => Directory.GetDirectories(directoryPath);

        public string[] GetFilesInDirectory(string directoryPath, string searchPattern) => Directory.GetFiles(directoryPath, searchPattern);

        public bool FileExists(string filePath) => File.Exists(filePath);

        public void HideFile(string filePath) => File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);

        public string ReadEntireFile(string filePath) => File.ReadAllText(filePath);

        public string SwapShortcutPath(string shortcutPath, string targetPath, bool preserveIcon) {
            var shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
            var oldDestination = shortcut.TargetPath;

            shortcut.TargetPath = targetPath;
            
            if (preserveIcon) {
                shortcut.IconLocation = oldDestination;
            }

            shortcut.Save();

            return oldDestination;
        }

        public void WriteAll(string filePath, string content) => File.WriteAllText(filePath, content);

        public TextWriter WriteTo(string filePath) => new StreamWriter(new FileStream(filePath, FileMode.Create));

    }
}

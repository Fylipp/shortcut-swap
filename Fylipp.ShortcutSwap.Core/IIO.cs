using System.IO;

namespace Fylipp.ShortcutSwap.Core {
    /// <summary>
    /// Defines all I/O operations.
    /// </summary>
    public interface IIO {

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="filePath">The file to delete.</param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Checks if a file exists.
        /// </summary>
        /// <param name="filePath">The path of the potential file.</param>
        /// <returns>Whether the file at the given path exists.</returns>
        bool FileExists(string filePath);

        /// <summary>
        /// Gets all the directories in a directory.
        /// </summary>
        /// <param name="directoryPath">The path of the directory.</param>
        /// <returns>All directories in the given directory.</returns>
        string[] GetDirectoriesInDirectory(string directoryPath);

        /// <summary>
        /// Gets all the files in a directory.
        /// </summary>
        /// <param name="directoryPath">The path of the directory.</param>
        /// <param name="searchPattern">The pattern that files must match.</param>
        /// <returns>All files in the given directory.</returns>
        string[] GetFilesInDirectory(string directoryPath, string searchPattern);

        /// <summary>
        /// Hides a file.
        /// </summary>
        /// <param name="filePath">The file to hide.</param>
        void HideFile(string filePath);

        /// <summary>
        /// Reads the entire content of a file.
        /// </summary>
        /// <param name="filePath">The file to read.</param>
        /// <returns>The file content.</returns>
        string ReadEntireFile(string filePath);

        /// <summary>
        /// Changes a shortcuts path.
        /// </summary>
        /// <param name="shortcutPath">The location of the shortcut.</param>
        /// <param name="targetPath">The new shortcut destination.</param>
        /// <param name="preserveIcon">Whether the icon path should be preserved.</param>
        /// <returns>The old destination of the shortcut.</returns>
        string SwapShortcutPath(string shortcutPath, string targetPath, bool preserveIcon);

        /// <summary>
        /// Writes the given content to the given file.
        /// </summary>
        /// <param name="filePath">The file to write to.</param>
        /// <param name="content">The content to write.</param>
        void WriteAll(string filePath, string content);

        /// <summary>
        /// Creates a writer that writes the the given path. Any existing file is overwritten.
        /// </summary>
        /// <param name="filePath">The path of the file to write to</param>
        /// <returns>The new writer.</returns>
        TextWriter WriteTo(string filePath);

    }
}

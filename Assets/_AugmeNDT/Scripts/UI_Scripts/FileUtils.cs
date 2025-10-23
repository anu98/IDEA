using System.IO;

public static class FileUtils
{
    public static void CopyDirectory(string sourceDir, string destDir, bool recursive = true)
    {
        if (!Directory.Exists(sourceDir))
            throw new DirectoryNotFoundException("Source directory not found: " + sourceDir);

        // Create destination directory if it doesn’t exist
        Directory.CreateDirectory(destDir);

        // Copy all files
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, true); // overwrite if exists
        }

        // Copy subdirectories
        if (recursive)
        {
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, destSubDir, true);
            }
        }
    }
}

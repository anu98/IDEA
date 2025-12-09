using System.IO;
using UnityEngine;

public static class PreviewPathHelper
{
    public static string GetPreviewPathForDataset(string datasetPath)
    {
        int hash = datasetPath.GetHashCode();
        string safeName = hash.ToString("X8");
        return Path.Combine(Application.persistentDataPath, "preview_" + safeName + ".png");
    }
}

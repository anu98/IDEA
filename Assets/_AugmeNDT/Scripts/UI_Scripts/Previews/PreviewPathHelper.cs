using System.Data;
using System.IO;
using UnityEngine;

public static class PreviewPathHelper
{
    public static string GetPreviewPathForDataset(string datasetPath)
    {
        string datasetName = Path.GetFileNameWithoutExtension(datasetPath);
        return datasetName + ".png";

        //string key = datasetPath.Replace("\\", "/");
        //int hash = key.GetHashCode();  // Editor-only hash
        //return "preview_" + hash.ToString("X8") + ".png";
    }

  



}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;   
//using System.IO;
//using TMPro;
//using System;

//namespace AugmeNDT
//{
//    public class PagePopulator : MonoBehaviour
//    {

//        public GameObject pagePrefab;
//        // public GameObject PreviewsOpenButton;

//        // Prefab for the page layout
//        public Transform contentPanel; // Panel where the pages will be instantiated
//        public string folderPath;
//        public List<GameObject> pages = new List<GameObject>(); // List to store the created pages



//        public void ClearPages()
//        {
//            foreach (var p in pages)
//            {
//                if (p != null)
//                    Destroy(p);
//            }
//            pages.Clear();
//        }


//        public void PopulatePages(string folderPath)
//        {
//            this.folderPath = folderPath;
//            // Check if the folder exists
//            if (!Directory.Exists(folderPath))
//            {
//                Debug.LogError("Folder does not exist: pagepopulator " + folderPath);
//                return;
//            }
//            Debug.Log($"[PagePopulator] PopulatePages() called with folderPath: {folderPath}");

//            // Get all files in the folder
//            string[] files = Directory.GetFiles(folderPath);



//            // Iterate through each file and create a page for it
//            foreach (string filePath in files)
//            {

//                // Instantiate page prefab
//                GameObject page = Instantiate(pagePrefab, contentPanel);
//                string fileName = Path.GetFileName(filePath);
//                page.name = fileName;
//                Debug.Log("Instantiated page: " + page.name);

//                pages.Add(page); // Add the page to the list



//                if (page == null)
//                {
//                    Debug.LogError("Failed to instantiate page prefab.");
//                    continue;
//                }

//                PageData pageData = page.GetComponent<PageData>();
//                if (pageData != null)
//                {



//                    // Debug.LogError("PageData component found on instantiated page!");
//                    // Debug.Log($"[PagePopulator] Calling SetFilePath with: {filePath}");

//                    pageData.SetFilePath(filePath);
//                    // Try to load existing preview PNG
//                    string normalizedPath = filePath.Replace("\\", "/");
//#if UNITY_ANDROID && !UNITY_EDITOR
//                // ANDROID / MAGIC LEAP: load pre-baked PNG from StreamingAssets/Previews
//                string fileName = PreviewPathHelper.GetPreviewFileName(normalizedPath);
//                string url = System.IO.Path.Combine(Application.streamingAssetsPath, "Previews/" + fileName);
//                Debug.Log($"[PagePopulator][ANDROID] Loading preview from {url}");

//                using (var req = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url))
//                {
//                    await req.SendWebRequest();
//                    if (req.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
//                    {
//                        Texture2D tex = DownloadHandlerTexture.GetContent(req);
//                        pageData.PreviewTexture = tex;

//                        var previewImage = page.transform
//                            .Find("Canvas/DynamicElements/PreviewImage")
//                            ?.GetComponent<UnityEngine.UI.RawImage>();

//                        if (previewImage != null)
//                        {
//                            Debug.Log($"[PagePopulator][ANDROID] Assigning preview texture on page {page.name}");
//                            previewImage.texture = tex;
//                        }
//                        else
//                        {
//                            Debug.LogError("[PagePopulator][ANDROID] PreviewImage RawImage not found on page: " + page.name);
//                        }
//                    }
//                    else
//                    {
//                        Debug.LogWarning($"[PagePopulator][ANDROID] Could not load preview for {fileName}: {req.error}");
//                    }
//                }


//            #else
//                    // EDITOR / OTHER PLATFORMS: keep your existing persistentDataPath logic
//                    string previewPath = PreviewPathHelper.GetPreviewPathForDataset(normalizedPath);
//                    Debug.Log($"[PagePopulator] Looking for preview at {previewPath}");

//                    if (File.Exists(previewPath))
//                    {
//                        Debug.Log($"[PagePopulator] Found preview PNG for {fileName}");

//                        byte[] bytes = File.ReadAllBytes(previewPath);
//                        Texture2D tex = new Texture2D(2, 2);
//                        tex.LoadImage(bytes);

//                        pageData.PreviewTexture = tex;

//                        var previewImage = page.transform
//                            .Find("Canvas/DynamicElements/PreviewImage")
//                            ?.GetComponent<UnityEngine.UI.RawImage>();

//                        if (previewImage != null)
//                        {
//                            Debug.Log($"[PagePopulator] Assigning preview texture on page {page.name}");
//                            previewImage.texture = tex;
//                        }
//                        else
//                        {
//                            Debug.LogError("PreviewImage RawImage not found under Canvas/DynamicElements on page: " + page.name);
//                        }
//                    }
//                    else
//                    {
//                        Debug.Log($"[PagePopulator] No preview PNG yet for {fileName}");
//                    }
//                }

//                }
//            }




//            public List<GameObject> GetPages()
//            {
//                return pages;
//            }



//            public void GetFileInfoAtIndex(int index)
//            {

//                // Get all files in the folder
//                string[] files = Directory.GetFiles(folderPath);

//                // Debug.LogError("Getfileinfoatindex");
//                FileInfo fileInfo = new FileInfo(files[index]);
//                string fileName = Path.GetFileName(files[index]);
//                string fileType = Path.GetExtension(files[index]);
//                string lastModified = fileInfo.LastWriteTime.ToString();
//                string fileSize = fileInfo.Length.ToString();

//                GameObject page = pages[index];

//                TextMeshProUGUI fileNameText = page.transform.Find("Canvas/DynamicElements/Name").GetComponent<TextMeshProUGUI>();
//                if (fileNameText == null)
//                {
//                    Debug.LogError("Could not find UI text component on page: " + page.name);
//                }
//                TextMeshProUGUI fileTypeText = page.transform.Find("Canvas/DynamicElements/Type").GetComponent<TextMeshProUGUI>();
//                TextMeshProUGUI lastModifiedText = page.transform.Find("Canvas/DynamicElements/LastModified").GetComponent<TextMeshProUGUI>();
//                TextMeshProUGUI fileSizeText = page.transform.Find("Canvas/DynamicElements/Size").GetComponent<TextMeshProUGUI>();

//                // Assign the retrieved values to the UI elements                    {

//                fileNameText.text = fileName; // Setting the fileName to the UI element

//                fileTypeText.text = fileType; // Setting the fileType to the UI element

//                lastModifiedText.text = lastModified; // Setting the lastModified to the UI element

//                fileSizeText.text = fileSize; // Setting the fileSize to the UI element






//            }





//        }
//    }
//#endif
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.Networking;

namespace AugmeNDT
{
    public class PagePopulator : MonoBehaviour
    {
        public GameObject pagePrefab;
        public Transform contentPanel;
        public string folderPath;
        public List<GameObject> pages = new List<GameObject>();

        public void ClearPages()
        {
            foreach (var p in pages)
            {
                if (p != null)
                    Destroy(p);
            }
            pages.Clear();
        }

        public void PopulatePages(string folderPath)
        {
            this.folderPath = folderPath;

            if (!Directory.Exists(folderPath))
            {
                Debug.LogError("Folder does not exist: pagepopulator " + folderPath);
                return;
            }
            Debug.Log($"[PagePopulator] PopulatePages() called with folderPath: {folderPath}");

            string[] files = Directory.GetFiles(folderPath);

            foreach (string filePath in files)
            {
                GameObject page = Instantiate(pagePrefab, contentPanel);
                string fileName = Path.GetFileName(filePath);
                page.name = fileName;
                Debug.Log("Instantiated page: " + page.name);

                pages.Add(page);

                if (page == null)
                {
                    Debug.LogError("Failed to instantiate page prefab.");
                    continue;
                }

                PageData pageData = page.GetComponent<PageData>();
                if (pageData == null)
                    continue;

                pageData.SetFilePath(filePath);
               
                string normalizedPath = filePath.Replace("\\", "/");
#if UNITY_ANDROID && !UNITY_EDITOR
string datasetName = Path.GetFileNameWithoutExtension(normalizedPath); 
Texture2D tex = Resources.Load<Texture2D>($"Previews/{datasetName}");

if (tex != null)
{
    pageData.PreviewTexture = tex;
    var previewImage = page.transform
        .Find("Canvas/DynamicElements/PreviewImage")
        ?.GetComponent<UnityEngine.UI.RawImage>();

    if (previewImage != null)
        previewImage.texture = tex;
    else
        Debug.LogError("PreviewImage RawImage not found on page: " + page.name);
}
else
{
    Debug.LogWarning($"Could not load preview for dataset: {datasetName}");
}
#else

                // EDITOR / OTHER PLATFORMS: keep existing persistentDataPath logic
                string previewPath = PreviewPathHelper.GetPreviewPathForDataset(normalizedPath);
                Debug.Log($"[PagePopulator] Looking for preview at {previewPath}");

                if (File.Exists(previewPath))
                {
                    Debug.Log($"[PagePopulator] Found preview PNG for {fileName}");

                    byte[] bytes = File.ReadAllBytes(previewPath);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);

                    pageData.PreviewTexture = tex;

                    var previewImage = page.transform
                        .Find("Canvas/DynamicElements/PreviewImage")
                        ?.GetComponent<UnityEngine.UI.RawImage>();

                    if (previewImage != null)
                    {
                        Debug.Log($"[PagePopulator] Assigning preview texture on page {page.name}");
                        previewImage.texture = tex;
                    }
                    else
                    {
                        Debug.LogError("PreviewImage RawImage not found under Canvas/DynamicElements on page: " + page.name);
                    }
                }
                else
                {
                    Debug.Log($"[PagePopulator] No preview PNG yet for {fileName}");
                }
#endif

            }



        }

        public List<GameObject> GetPages()
        {
            return pages;
        }

        public void GetFileInfoAtIndex(int index)
        {
            string[] files = Directory.GetFiles(folderPath);

            FileInfo fileInfo = new FileInfo(files[index]);
            string fileName = Path.GetFileName(files[index]);
            string fileType = Path.GetExtension(files[index]);
            string lastModified = fileInfo.LastWriteTime.ToString();
            string fileSize = fileInfo.Length.ToString();

            GameObject page = pages[index];

            TextMeshProUGUI fileNameText = page.transform.Find("Canvas/DynamicElements/Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI fileTypeText = page.transform.Find("Canvas/DynamicElements/Type").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI lastModifiedText = page.transform.Find("Canvas/DynamicElements/LastModified").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI fileSizeText = page.transform.Find("Canvas/DynamicElements/Size").GetComponent<TextMeshProUGUI>();

            fileNameText.text = fileName;
            fileTypeText.text = fileType;
            lastModifiedText.text = lastModified;
            fileSizeText.text = fileSize;
        }
    }
}



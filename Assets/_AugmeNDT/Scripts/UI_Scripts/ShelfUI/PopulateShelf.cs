using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

namespace AugmeNDT
{
    public class ShelfPopulator : MonoBehaviour
    {
        [SerializeField]
        private GameObject bookPrefab; // The book prefab to instantiate

        [SerializeField]
        private Transform[] slots;// Array to hold slot transforms

#if UNITY_EDITOR
        public string editorFolderPath = "D:/downloads/Main"; // Editor path
#else
        public string androidFolderPath =>
    "/storage/emulated/0/Main"; // Android path
#endif

        void Start()
        {
            string pathToUse = GetFolderPath();

            if (!Directory.Exists(pathToUse))
            {
                Debug.LogError("Dataset folder not found: " + pathToUse);
                return;
            }

            Debug.Log("Using dataset path: " + pathToUse);
            PopulateShelf(pathToUse);
        }

        string GetFolderPath()
        {
#if UNITY_EDITOR
            return editorFolderPath;
#else
            return androidFolderPath;
#endif
        }

        void PopulateShelf(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError("Directory does not exist: " + folderPath);
                return;
            }

            string[] folders = Directory.GetDirectories(folderPath);

            for (int i = 0; i < folders.Length && i < slots.Length; i++)
            {
                GameObject book = Instantiate(bookPrefab, slots[i].position, slots[i].rotation, slots[i]);
                // Optional: Set the name of the book to the folder name
                // Reset local scale to match the slot's scale
                book.transform.localScale = Vector3.one;
                book.name = Path.GetFileName(folders[i]);



                // Find the Text component at a specific path in the book prefab
                TextMeshPro bookText = book.transform.Find("BookName").GetComponent<TextMeshPro>(); // Change this to the actual p



                if (bookText != null)
                {
                    bookText.text = Path.GetFileName(folders[i]);
                }
                else
                {
                    Debug.LogError("Text component not found at the specified path in book prefab.");
                }
                //Assign the folder path to the BookClickHandler
               BookClickHandler clickHandler = book.GetComponent<BookClickHandler>();
                if (clickHandler != null)
                {
                    clickHandler.folderPath = folders[i];
                    Debug.Log("Set folder path: " + folders[i]);
                }
                else
                {
                    Debug.LogError("BookClickHandler component not found on the book object.");
                }

            }
        }
    }
}


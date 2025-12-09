using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AugmeNDT
{
    public class PageData : MonoBehaviour
    {
        string PageFilePath;
        [SerializeField] private Transform previewAnchor;
        public DataVisGroup CachedGroup { get; set; }
        public Texture2D PreviewTexture { get; set; }

        public GameObject PreviewObject { get; set; }// empty child on page for 3D preview// Store the file path of each dataset
        //void Start()
        //{
        //    Debug.Log($"Start() called on {gameObject.name}, filePath = '{filePath}'");

        //    //if (string.IsNullOrEmpty(filePath))
        //    //{
        //    //    Debug.LogError("PageData initialized WITHOUT a file path on pagedata script: " + gameObject.name);
        //    //}
        //    //else
        //    //{
        //    //    Debug.Log(" PageData loaded with file path on : " + gameObject.name + filePath);
        //    //}
        //}
        public void SetFilePath(string path)
        {
          

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($" SetFilePath() received an EMPTY path on {gameObject.name}");
            }
            PageFilePath = path;
            Debug.Log($"SetFilePath called on {gameObject.name}, setting path to: {PageFilePath}");
            //Debug.Log(PageFilePath); 

        }

        public string GetFilePath()
        {
            Debug.Log($"GetFilePath called on {gameObject.name}, getting path to: {PageFilePath}");
            if (string.IsNullOrEmpty(PageFilePath))
            {
                Debug.LogError($"GetFilePath() is returning an EMPTY value on {gameObject.name}");
            }
            return PageFilePath;
        }
        public Transform GetPreviewAnchor()
        {
            return previewAnchor != null ? previewAnchor : transform;
        }
    }
}

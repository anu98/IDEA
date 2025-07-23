using Unity.VisualScripting;
using UnityEngine;
//using static MagicLeap.SetupTool.Editor.NamespaceUpdater;

namespace AugmeNDT
{
    public class OpenDatasetButton : MonoBehaviour
    {
        private SceneObjectHandler sceneObjectHandler;

        GameObject PreviewButton;
        private void Start()
        {
            sceneObjectHandler = FindFirstObjectByType<SceneObjectHandler>();
            if (sceneObjectHandler == null)
            {
                Debug.LogError("sceneObjectHandler not found! Make sure GameManager has a reference to it.");
            }
        }

        public void OnPageClicked()
        {
            //GameObject clickedObject = gameObject;
            //GameObject canvass = transform.parent.gameObject;
            //GameObject pageTemplate = transform.parent.canvass;
            Transform pageTemplate = transform.parent.parent;
            Debug.Log("the clicked game object in pageBtnHandler " + pageTemplate);// The button you clicked
            //Debug.Log("Clicked on: " + clickedObject.name);
           // PageData pageData = clickedObject.transform.root.GetComponentInChildren<PageData>();

            PageData pageData = pageTemplate.GetComponent<PageData>(); ; // Find PageData in parent
            //PageData pageData = clickedObject.FindAnyObjectByType<PageData>();
            // Debug.Log("Button clicked!");
            //PageData pageData = GetComponent<PageData>();
            if (pageData == null) 
            {
                Debug.LogError("PageData component not found on the clicked page: ");
                return;
            }

            string path = pageData.GetFilePath();
            string correctedPath = path.Replace("\\", "/");

            Debug.Log("Calling SceneObjectHandler to load dataset from: " + correctedPath);
            sceneObjectHandler.LoadPreSelectedObject(correctedPath);


        }
    }
}
